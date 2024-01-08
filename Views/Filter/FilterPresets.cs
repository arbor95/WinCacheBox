using System;

namespace WinCachebox
{
    class FilterPresets
  {
      static String attspreset = "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";
      public static String[] presets = new String[] {
            // All Caches
            "0,0,0,0,0,0,0,0,0,1,5,1,5,0,4,0,5,True,True,True,True,True,True,True,True,True,True,True," +  attspreset + ",,,,,,0,", 

            // All Caches to find
            "-1,-1,-1,-1,0,0,0,0,0,1,5,1,5,0,4,0,5,True,True,True,True,True,True,True,True,True,True,True," +  attspreset + ",,,,,,0,",

            // Quick Cache
            "-1,-1,-1,-1,0,0,0,0,0,1,2.5,1,2.5,0,4,0,5,True,False,False,True,True,False,False,False,False,False,False," +  attspreset + ",,,,,,0,",

            // Fetch some Travelbugs
            "-1,-1,0,0,1,0,0,0,0,1,3,1,3,0,4,0,5,True,False,False,False,False,False,False,False,False,False,False," +  attspreset + ",,,,,,0,",

            // Drop off Travelbugs
            "-1,-1,0,0,0,0,0,0,0,1,3,1,3,2,4,0,5,True,False,False,False,False,False,False,False,False,False,False," +  attspreset + ",,,,,,0,",

            // Highlights
            "-1,-1,0,0,0,0,0,0,0,1,5,1,5,0,4,3.5,5,True,True,True,True,True,True,True,True,True,True,True," +  attspreset + ",,,,,,0,",

            // Favoriten
            "0,0,0,0,0,1,0,0,0,1,5,1,5,0,4,0,5,True,True,True,True,True,True,True,True,True,True,True," +  attspreset + ",,,,,,0,",

            // prepare to archive
            "0,0,-1,-1,0,-1,-1,-1,0,1,5,1,5,0,4,0,5,True,True,True,True,True,True,True,True,True,True,True," +  attspreset + ",,,,,,0,",
            
            // Listing Changed
            "0,0,0,0,0,0,0,1,0,1,5,1,5,0,4,0,5,True,True,True,True,True,True,True,True,True,True,True," +  attspreset + ",,,,,,0,"

      };

    public FilterPresets()
    {
    }
  }
}
