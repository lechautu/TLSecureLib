package com.example.androidsigcheck;

import android.content.Context;
import android.content.pm.PackageManager;
import android.content.pm.Signature;
import android.util.Log;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;

public class SignatureValidation {
    public static byte[] getSignature(Context context) {
        byte[] signature = null;
        try {
            Signature[] sigs = context.getPackageManager().getPackageInfo(context.getPackageName(), PackageManager.GET_SIGNATURES).signatures;
            for (Signature sig: sigs) {
		try {
			MessageDigest md = MessageDigest.getInstance("SHA-256");
			signature = md.digest(sig.toByteArray());
		} catch (NoSuchAlgorithmException e) { }
            }
        } catch (PackageManager.NameNotFoundException e) {
            e.printStackTrace();
        }
        return signature;
    }
}
