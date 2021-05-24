# pulse
## How to add your own python script module.
### Rules
Python script should save the result of the analysis as JSON file in the next folder: `C:\Users\Admin\AppData\Roaming\pulse`. 
[!] The name of the saved file should be as the input filename with prefix `ANALYSIS_NAME`_FILENAME.
For the input the program will give you a path to a signal `.txt` file as an argument.
### Keys
There is two acceptable data types:
`{"type": "graphical"}` or `{"type": "tabular"}`
Next attribute `{"data": ...}` is required.
Child of `{"data": ...}` will be `{"charts": []}` if it is a `{"type": "graphical"}`, otherwise you can just add the results of the analysis `{"parametr1": "value1", "parametr2": "value2", etc ...}`
The attributes of chart object in `{"charts": []}` is next:
`{"type": "CHART_TYPE", "color": "COLOR_NAME", "markerStyle": "MARKER_STYLE", "markerSize": int, "x": [], "y: []}`, all available options you can see below, beside COLOR_NAME, programm will find color by name for example:`red, green, blue`and etc.

### CHART_TYPE
1. scatter
2. bar
3. line
4. area
5. boxplot
6. pie

### MARKER_STYLE
1. circle
2. cross
3. diamond
4. star
5. triangle
6. squere

## Examples
**Grapical:**
```json
{
  "type":"graphical",
  "data":{
    "charts":[
      {
        "type":"scatter",
        "color":"red",
        "markerStyle":"circle",
        "markerSize":10,
        "x":[
          1,
          2,
          3,
          4
        ],
        "y":[
          1,
          2,
          3,
          4
        ]
      },
      {
        "type":"line",
        "color":"blue",
        "x":[
          1,
          2,
          3,
          4
        ],
        "y":[
          1,
          2,
          3,
          4
        ]
      }
    ]
  }
}
```
**Table:**
```json
{
  "type":"table",
  "data":{
    "IC":32.907451029206918,
    "(LF/HF)av":0.63430649508275283,
    "ULFav":0.0060460735828449146,
    "VLFav":0.0730950045118496,
    "LFav":2.4053702814533495,
    "HFav":3.7921262041302923,
    "ULF":0.096326632237495269,
    "VLF":1.1645567195855942,
    "LF":38.322593220496792,
    "HF":60.416523427680104,
    "TP":6.2766375636783369,
    "SI":0.0042032163742690056
  }
}
```
