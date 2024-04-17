package com.unity3d.player;
import com.unity3d.player.UnityPlayerActivity;
import android.os.Build;
import android.os.Bundle;
import android.app.admin.DevicePolicyManager;
import android.app.ActivityOptions;
import android.content.ComponentName;
import android.content.Context;
import android.util.Log;
import android.widget.Toast;
import android.view.View;
import android.view.WindowManager;

public class custActiv extends UnityPlayerActivity {
  protected void onCreate(Bundle savedInstanceState) {
    // Calls UnityPlayerActivity.onCreate()
    super.onCreate(savedInstanceState);
    // Prints debug message to Logcat
    String[] APP_PACKAGES = {"com.DefaultCompany.LiteraryTriangle"};
    //Context context = new ComponentName(getApplicationContext(), custActiv.class);
    /*DevicePolicyManager dpm = (DevicePolicyManager) getApplicationContext().getSystemService(Context.DEVICE_POLICY_SERVICE);
    ComponentName adminName = new ComponentName(getApplicationContext(), custActiv.class);
    if(dpm!=null && adminName!=null)
        dpm.setLockTaskPackages(adminName, APP_PACKAGES);
    //Log.d("OverrideActivity", "onCreate called!");
    //Toast.makeText(getApplicationContext(), "ewewe", Toast.LENGTH_SHORT).show();
    ActivityOptions options = ActivityOptions.makeBasic();
    options.setLockTaskEnabled(true);*/
    DevicePolicyManager mDPM = (DevicePolicyManager) getSystemService(Context.DEVICE_POLICY_SERVICE);
    ComponentName mDeviceAdminSample = new ComponentName(this, custActiv.class);
    if(mDPM == null){
        //Toast.makeText(getApplicationContext(), "7877766", Toast.LENGTH_SHORT).show();
    }
    //mDPM.setActiveAdmin(mDeviceAdminSample, true, 11822);
    //mDPM.setLockTaskPackages(mDeviceAdminSample, APP_PACKAGES);
    if (mDPM.isDeviceOwnerApp(this.getPackageName())) {
      //Log.d("isDeviceOwnerApp: YES");
      //Toast.makeText(getApplicationContext(), "1", Toast.LENGTH_SHORT).show();
      String[] packages = {this.getPackageName()};
      mDPM.setLockTaskPackages(mDeviceAdminSample, packages);
    } else {
      //Log.d("isDeviceOwnerApp: NO");
      //Toast.makeText(getApplicationContext(), "12", Toast.LENGTH_SHORT).show();
    }

    if (mDPM.isLockTaskPermitted(this.getPackageName())) {
      //Log.d("isLockTaskPermitted: ALLOWED");
      //Toast.makeText(getApplicationContext(), "2", Toast.LENGTH_SHORT).show();
      startLockTask();
    } else {
      //Log.d("isLockTaskPermitted: NOT ALLOWED");
      //Toast.makeText(getApplicationContext(), "22", Toast.LENGTH_SHORT).show();
    }
  }
  public void onBackPressed()
  {
    // Instead of calling UnityPlayerActivity.onBackPressed(), this example ignores the back button event
    //Toast.makeText(getApplicationContext(), "Bckww", Toast.LENGTH_SHORT).show();
    // super.onBackPressed();
  }
  public void onHomePressed() {
    //Log.d("Pressed", "Home Button Pressed");
    //Toast.makeText(getApplicationContext(), "HOME111", Toast.LENGTH_SHORT).show();
    //super.onHomePressed();
  }
}