package com.checkinn.front.activities;

import android.arch.persistence.room.Room;
import android.content.Intent;
import android.os.Bundle;
import android.provider.MediaStore;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.ExpandableListAdapter;
import android.widget.ExpandableListView;

import com.checkinn.front.R;
import com.checkinn.front.adapters.ItemListExpandableAdapter;
import com.checkinn.front.database.database.AppDatabase;
import com.checkinn.front.database.entities.Item;
import com.checkinn.front.rest.OnResourceLoad;
import com.checkinn.front.rest.RestItemLoader;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class MainActivity extends AppCompatActivity {

    private static final int REQUEST_IMAGE_CAPTURE = 1;

    private RestItemLoader restItemLoader;

    private List<String> listDataHeader;
    private HashMap<String, List<String>> listDataChildren;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        Toolbar toolbar = findViewById(R.id.toolbar);
        AppDatabase db = Room.databaseBuilder(getApplicationContext(),
                AppDatabase.class, "database").allowMainThreadQueries().build();
        startLogin();

        restItemLoader = new RestItemLoader();
        populateExpandableListView();

        //button for camera
        FloatingActionButton fab = findViewById(R.id.fab);
        fab.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                MainActivity.this.dispatchTakePictureIntent();
            }
        });
    }

    protected void startLogin() {
        Intent intent = new Intent(this, LoginActivity.class);
        startActivity(intent);
    }

    private void populateExpandableListView() {
        ExpandableListView expandableListView = findViewById(R.id.expandableListView);

        prepareExpandableListData();

        ExpandableListAdapter listAdapter = new ItemListExpandableAdapter(this, listDataHeader, listDataChildren);

        expandableListView.setAdapter(listAdapter);
    }

    private void prepareExpandableListData() {
        listDataHeader = new ArrayList<>();
        listDataChildren = new HashMap<>();

        restItemLoader.Load(new OnResourceLoad<List<Item>>() {
            @Override
            public void OnLoad(List<Item> items) {
                if (items.size() == 0) {
                    listDataHeader.add("No data");
                }

                for (int i = 0; i < items.size(); i++) {
                    listDataHeader.add(items.get(i).itemName);
                    List<String> data = new ArrayList<String>();
                    data.add("Shop where price is lowest: " + items.get(i).shopName);
                    data.add("Lowest price: " + items.get(i).price + " €");
                    listDataChildren.put(listDataHeader.get(i), data);
                }
            }
        });
    }

    private void dispatchTakePictureIntent() {
        Intent takePictureIntent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
        if (takePictureIntent.resolveActivity(getPackageManager()) != null) {
            startActivityForResult(takePictureIntent, REQUEST_IMAGE_CAPTURE);
        }
    }

}
