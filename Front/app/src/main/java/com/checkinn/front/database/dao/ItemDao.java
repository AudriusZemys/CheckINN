package com.checkinn.front.database.dao;

import android.arch.persistence.room.Dao;
import android.arch.persistence.room.Delete;
import android.arch.persistence.room.Insert;
import android.arch.persistence.room.OnConflictStrategy;
import android.arch.persistence.room.Query;
import android.arch.persistence.room.Update;

import com.checkinn.front.database.entities.Item;

@Dao
public interface ItemDao {
    @Insert(onConflict = OnConflictStrategy.REPLACE)
    public void insertItems(Item... items);

    @Update
    public void updateItems(Item... items);

    @Delete
    public void deleteItems(Item... items);

    @Query("SELECT * FROM item")
    public Item[] loadAllItems();

    @Query("delete from item")
    public void deleteAllItems();
}
