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

    private int LOGGEDON = 0;
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


        //testing database access
        Item item = new Item();
        item.shopName = "shop";
        item.itemName = "amazing_item";
        item.price = 25.15;
//        db.itemDao().insertItems(item);
//        Item[] items = db.itemDao().loadAllItems();

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

        for (int i = 0; i < items.length; i++) {
            listDataHeader.add(items[i].itemName);
            List<String> data = new ArrayList<String>();
            data.add("Shop where price is lowest: " + items[i].shopName);
            data.add("Lowest price: " + items[i].price + " €");
            listDataChildren.put(listDataHeader.get(i), data);
        }

//            // Adding child data
//            listDataHeader.add("Top 250");
//            listDataHeader.add("Now Showing");
//            listDataHeader.add("Coming Soon..");
//
//            // Adding child data
//            List<String> top250 = new ArrayList<String>();
//            top250.add("The Shawshank Redemption");
//            top250.add("The Godfather");
//            top250.add("The Godfather: Part II");
//            top250.add("Pulp Fiction");
//            top250.add("The Good, the Bad and the Ugly");
//            top250.add("The Dark Knight");
//            top250.add("12 Angry Men");
//
//            List<String> nowShowing = new ArrayList<String>();
//            nowShowing.add("The Conjuring");
//            nowShowing.add("Despicable Me 2");
//            nowShowing.add("Turbo");
//            nowShowing.add("Grown Ups 2");
//            nowShowing.add("Red 2");
//            nowShowing.add("The Wolverine");
//
//            List<String> comingSoon = new ArrayList<String>();
//            comingSoon.add("2 Guns");
//            comingSoon.add("The Smurfs 2");
//            comingSoon.add("The Spectacular Now");
//            comingSoon.add("The Canyons");
//            comingSoon.add("Europa Report");
//
//            listDataChildren.put(listDataHeader.get(0), top250); // Header, Child data
//            listDataChildren.put(listDataHeader.get(1), nowShowing);
//            listDataChildren.put(listDataHeader.get(2), comingSoon);
    }

}
