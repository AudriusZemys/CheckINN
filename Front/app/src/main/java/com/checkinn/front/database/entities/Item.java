package com.checkinn.front.database.entities;


public class Item {
    public String itemName;
    public String shopName;
    public double price;

    public Item(String itemName, String shopName, double price) {
        this.itemName = itemName;
        this.shopName = shopName;
        this.price = price;
    }
}