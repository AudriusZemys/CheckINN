package com.checkinn.front.database.database;

import android.arch.persistence.room.Database;
import android.arch.persistence.room.RoomDatabase;

import com.checkinn.front.database.dao.ItemDao;
import com.checkinn.front.database.entities.Item;

@Database(entities = {Item.class}, version = 1)
public abstract class AppDatabase extends RoomDatabase {
    public abstract ItemDao userDao();
}
