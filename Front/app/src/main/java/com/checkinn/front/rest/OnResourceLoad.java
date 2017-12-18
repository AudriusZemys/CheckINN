package com.checkinn.front.rest;

/**
 * Created by lpraninskas on 12/18/17.
 */

public interface OnResourceLoad<T> {
    void OnLoad(T t);
}
