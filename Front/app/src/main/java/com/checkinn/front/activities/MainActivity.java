package com.checkinn.front.activities;

import android.arch.persistence.room.Room;
import android.content.Intent;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.View;

import com.checkinn.front.R;
import com.checkinn.front.database.database.AppDatabase;
import com.checkinn.front.database.entities.Item;

public class MainActivity extends AppCompatActivity {

    private int LOGGEDON = 0;

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
        startLogin();


        //bad practice running on main thread but whatever
        AppDatabase db = Room.databaseBuilder(getApplicationContext(),
                AppDatabase.class, "database").allowMainThreadQueries().build();

        //testing database access
        Item item = new Item();
        item.shopName = "shop";
        item.itemName = "amazing_item";
        item.price = 25.15;
        db.itemDao().insertItems(item);
        Item[] items = db.itemDao().loadAllItems();

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

}
