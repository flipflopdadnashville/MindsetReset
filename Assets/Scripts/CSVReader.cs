using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;

public class CSVReader : MonoBehaviour
{
    // static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    // static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    // static char[] TRIM_CHARS = { '\"' };

    // public async Task<List<Dictionary<string, object>>> Read (string file)
    // {
    //     if(!System.IO.File.Exists(file)) 
    //     {
    //             Debug.LogWarning($"This file path is invalid: '{file}'");
    //             return null;
    //     }
    //     // we're still on unity thread here
    //     return await Task.Run(
    //         () =>
    //         {
    //             // we're on worker thread, so exciting!
    //             string fileContent = System.IO.File.ReadAllText(file);
    //             TextAsset data = new TextAsset(fileContent);
    //             var list = new List<Dictionary<string, object>>();

    //             var lines = Regex.Split (data.text, LINE_SPLIT_RE);
 
    //             if(lines.Length <= 1) return list;
        
    //             var header = Regex.Split(lines[0], SPLIT_RE);
    //             for(var i=1; i < lines.Length; i++) {
        
    //                 var values = Regex.Split(lines[i], SPLIT_RE);
    //                 if(values.Length == 0 ||values[0] == "") continue;
        
    //                 var entry = new Dictionary<string, object>();
    //                 for(var j=0; j < header.Length && j < values.Length; j++ ) {
    //                     string value = values[j];
    //                     value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
    //                     object finalvalue = value;
    //                     int n;
    //                     float f;
    //                     if(int.TryParse(value, out n)) {
    //                         finalvalue = n;
    //                     } else if (float.TryParse(value, out f)) {
    //                         finalvalue = f;
    //                     }
    //                     entry[header[j]] = finalvalue;
    //                 }
    //                 list.Add (entry);
    //             }

    //             return list;
    //         }
    //     );
    //     // back on unity thread
    // }

    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };
 
    public TextAsset LoadData(string file){
        string FileContent = File.ReadAllText(file);
        TextAsset data = new TextAsset(FileContent);
        return data;
    }

    public List<Dictionary<string, object>> Read(TextAsset data, int startLine, int endLine, bool allData)
    {
        var list = new List<Dictionary<string, object>>(); 
        var lines = Regex.Split (data.text, LINE_SPLIT_RE);
        //Debug.Log("From CSVReader Read, data length is: " + lines.Length);
        if(lines.Length <= 1) return list;
 
        var header = Regex.Split(lines[0], SPLIT_RE);
        if(allData == false){
            if(endLine > lines.Length)
            {
                endLine = lines.Length;
            }

            for(var i=startLine; i < endLine; i++) {
    
                var values = Regex.Split(lines[i], SPLIT_RE);
                if(values.Length == 0 ||values[0] == "") continue;
    
                var entry = new Dictionary<string, object>();
                for(var j=0; j < header.Length && j < values.Length; j++ ) {
                    string value = values[j];
                    value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                    object finalvalue = value;
                    int n;
                    float f;
                    if(int.TryParse(value, out n)) {
                        finalvalue = n;
                    } else if (float.TryParse(value, out f)) {
                        finalvalue = f;
                    }
                    entry[header[j]] = finalvalue;
                }
                list.Add (entry);
            }
        }
        else if(allData == true){
            for(var i= 1; i < lines.Length; i++) {
                var values = Regex.Split(lines[i], SPLIT_RE);
                if(values.Length == 0 ||values[0] == "") continue;
    
                var entry = new Dictionary<string, object>();
                for(var j=0; j < header.Length && j < values.Length; j++ ) {
                    string value = values[j];
                    value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                    object finalvalue = value;
                    int n;
                    float f;
                    if(int.TryParse(value, out n)) {
                        finalvalue = n;
                    } else if (float.TryParse(value, out f)) {
                        finalvalue = f;
                    }
                    entry[header[j]] = finalvalue;
                }
                list.Add (entry);
            }            
        }

        list = list.OrderBy(i => Guid.NewGuid()).ToList();

        return list;
    }
}