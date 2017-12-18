package com.checkinn.front.rest;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.Path;

/**
 * Created by lpraninskas on 12/18/17.
 */

public interface RestProductsService {
    @GET("products/GetAll")
    Call<List<Product>> listProducts();

    @POST("products/GetByCheckId/{checkID}")
    Call<List<Product>> productsByCheckId(@Path("checkId") int checkId);
}

