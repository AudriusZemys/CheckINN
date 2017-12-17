package com.checkinn.front.database.entities;

import android.arch.persistence.room.ColumnInfo;
import android.arch.persistence.room.Entity;
import android.arch.persistence.room.PrimaryKey;

@Entity(tableName = "item")
class Item {
    @PrimaryKey(autoGenerate = true)
    public int id;
    @ColumnInfo(name = "item_name")
    public String itemName;
    @ColumnInfo(name = "shop_name")
    public String shopName;
    @ColumnInfo(name = "price")
    public double price;

//    public Item(String itemName, String shopName, double price) {
//        this.itemName = itemName;
//        this.shopName = shopName;
//        this.price = price;
//    }
}