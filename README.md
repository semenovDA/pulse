# pulse
## How to add your own python script module.
### Rules
Python script should save the result of the analysis as JSON file in the next folder: `C:\Users\Admin\AppData\Roaming\pulse`. 
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
