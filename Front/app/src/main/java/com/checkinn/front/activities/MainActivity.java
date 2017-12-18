package com.checkinn.front.activities;

import android.arch.persistence.room.Room;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.os.Environment;
import android.provider.MediaStore;
import android.support.design.widget.FloatingActionButton;
import android.support.v4.content.FileProvider;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.View;
import android.widget.ExpandableListAdapter;
import android.widget.ExpandableListView;

import com.checkinn.front.R;
import com.checkinn.front.adapters.ItemListExpandableAdapter;
import com.checkinn.front.database.database.AppDatabase;
import com.checkinn.front.database.entities.Item;

import java.io.File;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.List;

import static android.os.Environment.getExternalStoragePublicDirectory;

public class MainActivity extends AppCompatActivity {

    private static final int REQUEST_IMAGE_CAPTURE = 1;
    private static final int REQUEST_TAKE_PHOTO = 1;
    private AppDatabase db;
    private ExpandableListAdapter listAdapter;
    private ExpandableListView expandableListView;
    private List<String> listDataHeader;
    private HashMap<String, List<String>> listDataChildren;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        //some sort of style mismatch, bbz
        //setSupportActionBar(toolbar);

//        if (savedInstanceState.getInt("LOGGED") == 0) {
//
//        }
        //bad practice running on main thread but whatever
        db = Room.databaseBuilder(getApplicationContext(),
                AppDatabase.class, "database").allowMainThreadQueries().build();
        startLogin();
        populateExpandableListView();


        dropTableAndInsertTestData();


        //button for camera
        FloatingActionButton fab = (FloatingActionButton) findViewById(R.id.fab);
        fab.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
//                Snackbar.make(view, "Replace with your own action", Snackbar.LENGTH_LONG)
//                        .setAction("Action", null).show();
                dispatchTakePictureIntent();
            }
        });
    }

    protected void startLogin() {
        Intent intent = new Intent(this, LoginActivity.class);
        startActivity(intent);
    }

    private void populateExpandableListView() {
        expandableListView = (ExpandableListView) findViewById(R.id.expandableListView);

        prepareExpandableListData();

        listAdapter = new ItemListExpandableAdapter(this, listDataHeader, listDataChildren);

        expandableListView.setAdapter(listAdapter);
    }

    private void prepareExpandableListData() {
        listDataHeader = new ArrayList<String>();
        listDataChildren = new HashMap<String, List<String>>();

        Item[] items = db.itemDao().loadAllItems();

        if (items.length == 0) {
            listDataHeader.add("No data");
        }

        for (int i = 0; i < items.length; i++) {
            listDataHeader.add(items[i].itemName);
            List<String> data = new ArrayList<String>();
            data.add("Shop where price is lowest: " + items[i].shopName);
            data.add("Lowest price: " + items[i].price + " €");
            listDataChildren.put(listDataHeader.get(i), data);
        }
    }

    //possibly remove from final product
    private void dropTableAndInsertTestData() {

        db.itemDao().deleteAllItems();

        Item item;
        item = new Item();
        item.shopName = "Maxima";
        item.itemName = "Citrinos";
        item.price = 1.39;
        db.itemDao().insertItems(item);

        item = new Item();
        item.shopName = "Maxima";
        item.itemName = "Malta kava JACOBS KRONUNG";
        item.price = 4.99;
        db.itemDao().insertItems(item);

        item = new Item();
        item.shopName = "Maxima";
        item.itemName = "Kepintos ir sūdytos pistacijos";
        item.price = 14.74;
        db.itemDao().insertItems(item);

        item = new Item();
        item.shopName = "Maxima";
        item.itemName = "Brendis J. P. CHENET RESERVE IMPERIALE";
        item.price = 16.99;
        db.itemDao().insertItems(item);

        item = new Item();
        item.shopName = "Maxima";
        item.itemName = "Kepintos saulėgrąžos žM";
        item.price = 1.48;
        db.itemDao().insertItems(item);

        item = new Item();
        item.shopName = "Maxima";
        item.itemName = "Tortas JUODOJI ROŽĖ";
        item.price = 6.97;
        db.itemDao().insertItems(item);
    }

    private String mCurrentPhotoPath;

    private File createImageFile() throws IOException {
        // Create an image file name
        String timeStamp = new SimpleDateFormat("yyyyMMdd_HHmmss").format(new Date());
        String imageFileName = "JPEG_" + timeStamp + "_";
        //store this in public folder
        File storageDir = getExternalFilesDir(Environment.DIRECTORY_PICTURES);
//        File storageDir = getExternalStoragePublicDirectory(Environment.DIRECTORY_PICTURES);
//        make sure that dirs exist
        storageDir.mkdirs();
        File image = File.createTempFile(
                imageFileName,  /* prefix */
                ".jpg",         /* suffix */
                storageDir      /* directory */
        );

        // Save a file: path for use with ACTION_VIEW intents
        mCurrentPhotoPath = image.getAbsolutePath();
        return image;
    }

    private void dispatchTakePictureIntent() {
        Intent takePictureIntent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
        // Ensure that there's a camera activity to handle the intent
        if (takePictureIntent.resolveActivity(getPackageManager()) != null) {
            // Create the File where the photo should go
            File photoFile = null;
            try {
                photoFile = createImageFile();
            } catch (IOException ex) {
                // Error occurred while creating the File
                //...
                Log.e("MYAPP", "exception", ex);
            }
            // Continue only if the File was successfully created
            if (photoFile != null) {
                Uri photoURI = FileProvider.getUriForFile(this,
                        "com.example.android.fileprovider",
                        photoFile);
                takePictureIntent.putExtra(MediaStore.EXTRA_OUTPUT, photoURI);
                startActivityForResult(takePictureIntent, REQUEST_TAKE_PHOTO);
            }
        }
        galleryAddPic();
    }

    private void galleryAddPic() {
        Intent mediaScanIntent = new Intent(Intent.ACTION_MEDIA_SCANNER_SCAN_FILE);
        File f = new File(mCurrentPhotoPath);
        Uri contentUri = Uri.fromFile(f);
        mediaScanIntent.setData(contentUri);
        this.sendBroadcast(mediaScanIntent);
    }
}
