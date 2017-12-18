package com.checkinn.front.rest;

import com.google.gson.annotations.SerializedName;

public class Product {

    @SerializedName("ProductEntry")
    private String Name;

    @SerializedName("Cost")
    private Double Price;

    public String getName() {
        return Name;
    }

    public void setName(String name) {
        Name = name;
    }

    public Double getPrice() {
        return Price;
    }

    public void setPrice(Double price) {
        Price = price;
    }
}
