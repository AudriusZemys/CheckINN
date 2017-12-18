package com.checkinn.front.rest;

import com.checkinn.front.database.entities.Item;

import java.util.List;
import java.util.concurrent.Callable;
import java.util.function.Predicate;

/**
 * Created by lpraninskas on 12/18/17.
 */

public interface Loader<T> {
    boolean Load(OnResourceLoad<List<T>> callback);
}

