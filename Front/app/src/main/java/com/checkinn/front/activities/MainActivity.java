package com.checkinn.front.activities;

import android.arch.persistence.room.Room;
import android.content.Intent;
import android.os.Bundle;
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

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class MainActivity extends AppCompatActivity {

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


        //login button for camera
        FloatingActionButton fab = (FloatingActionButton) findViewById(R.id.fab);
        fab.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Snackbar.make(view, "Replace with your own action", Snackbar.LENGTH_LONG)
                        .setAction("Action", null).show();
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

}
