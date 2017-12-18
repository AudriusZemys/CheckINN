package com.checkinn.front.rest;

import android.os.Build;
import android.support.annotation.RequiresApi;

import com.checkinn.front.database.entities.Item;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.Callable;
import java.util.function.Function;
import java.util.function.Predicate;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class RestItemLoader implements Loader<Item> {

    @Override
    public boolean Load(final OnResourceLoad<List<Item>> callback) {
        Retrofit retrofit = new Retrofit.Builder()
                .baseUrl("http://193.219.91.103:9384/api/")
                .addConverterFactory(GsonConverterFactory.create())
                .build();
        RestProductsService productsService = retrofit.create(RestProductsService.class);

        productsService.listProducts().enqueue(new Callback<List<Product>>() {

            @Override
            public void onResponse(Call<List<Product>> call, Response<List<Product>> response) {
                List<Item> items = new ArrayList<>();
                for (Product product : response.body()) {
                    items.add(new Item(product.getName(), "Maxima", product.getPrice()));
                }
                callback.OnLoad(items);
            }

            @Override
            public void onFailure(Call<List<Product>> call, Throwable t) {
            }
        });
        return true;
    }
}
