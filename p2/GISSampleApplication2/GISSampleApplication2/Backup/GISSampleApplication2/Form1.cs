using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GISSampleApplication2
{
    public partial class Form1 : Form
    {
        //Create a layer handler
        public int intHandler;
        //Create a new instance for MapWinGIS.Shapefile
        public MapWinGIS.Shapefile myShapefile = new MapWinGIS.Shapefile();

        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            //Define the data source of Shapefile instanse
            myShapefile.Open(@"D:\GISSampleData2\arabcntry.shp", null);
            //Display the layer on the map control
            intHandler = axMap1.AddLayer(myShapefile, true);
            
            SetBasicSymbology();

        }        

        public void SetBasicSymbology()
        {
            //Set Filling color of the ploygon shapefile
            axMap1.set_ShapeLayerFillColor(intHandler,
                (UInt32)(System.Drawing.ColorTranslator.ToOle
                        (System.Drawing.Color.Honeydew)));
            //Set the outline color of the polygon features
            axMap1 .set_ShapeLayerLineColor(intHandler ,
                (UInt32)(System.Drawing.ColorTranslator.ToOle
                        (System.Drawing.Color.Goldenrod)));
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            SetBasicSymbology();
        }

        private void btnQuant_Click(object sender, EventArgs e)
        {
            //Create a new instance for MapWinGIS.Table
            MapWinGIS.Table myTable = new MapWinGIS.Table();
            //Define the data source of Table instance
            myTable.Open(@"D:\GISSampleData2\arabcntry.dbf", null);
            //Define the index of the field will used in symbology
            int myFieldIndex = 1;
            //Get the number of rows in the table
            int numberOfRows = myTable.NumRows;
            //Create an array to store the cell values of the field
            double[] myCellsValues = new double[numberOfRows];
            //Populate the array with cell values restored from the Table instance
            for (int i = 0; i < numberOfRows - 1; i++)
            {
                myCellsValues[i] =
                    System.Convert.ToDouble(myTable.get_CellValue(1, i));
            }
            //Get the minimum and maximum values
            double minValue = myCellsValues.Min();
            double maxValue = myCellsValues.Max();

            //Create a new instance for MapWinGIS.ShapefileColorScheme
            MapWinGIS.ShapefileColorScheme myScheme = new MapWinGIS.ShapefileColorScheme();
            //Set the layer handler to the MapWinGIS.ShapefileColorScheme instance
            myScheme.LayerHandle = intHandler;
            //Set the field index to use in symbology
            myScheme.FieldIndex = myFieldIndex;
            //Create a new instance for MapWinGIS.ShapefileColorBreak 
            MapWinGIS.ShapefileColorBreak myBreak = new MapWinGIS.ShapefileColorBreak();
            //Set the minimum value in the field as a start value
            myBreak.StartValue = minValue;
            //Set the start color of the scheme
            myBreak.StartColor =
                (UInt32)(System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua));
            //Set the maximum value in the field as an end value
            myBreak.EndValue = maxValue;
            //Set the end color of the sceme
            myBreak.EndColor =
                (UInt32)(System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkBlue));
            //Add the break to the color scheme
            myScheme.Add(myBreak);
            //Upgrade display using the scheme
            axMap1.ApplyLegendColors(myScheme);
        }

        private void btnLabel_Click(object sender, EventArgs e)
        {
            //Define a string to store the label of the feature
            string myLabel;
            //define two double variables to use as a coordinates for label postion
            double x, y;
            //Use a loop to read the cell value that use as a lable and set the label on the map
            for (int i = 0; i < myShapefile.NumShapes -1; i++) 
            {
                //assign the value of the cells of the field number zero to the label
                myLabel = System.Convert.ToString(
                    myShapefile.get_CellValue(0, i)); 
                //Calculate the x position for the label
                x = myShapefile.get_Shape(i).Extents.xMin +
                    (myShapefile.get_Shape(i).Extents.xMax - myShapefile.get_Shape(i).Extents.xMin) / 2;
                //Calculate the y postion for the label
                y = myShapefile.get_Shape(i).Extents.yMin +
                    (myShapefile.get_Shape(i).Extents.yMax - myShapefile.get_Shape(i).Extents.yMin) / 2;
                //add the label on the map
                axMap1.AddLabel(
                    intHandler, //Layer handler
                    myLabel, //Label string
                    (UInt32)(System.Drawing.ColorTranslator.ToOle
                        (System.Drawing.Color.Black)), //Color of the label
                        x, y, //Position of the lable
                        MapWinGIS.tkHJustification.hjCenter //Justification of the label
                        );
            }
        }

        private void toolCursor_Click(object sender, EventArgs e)
        {
            axMap1.CursorMode = MapWinGIS.tkCursorMode.cmNone;
        }

        private void toolZoomExtent_Click(object sender, EventArgs e)
        {
            axMap1.ZoomToMaxExtents();
        }

        private void toolZoomIn_Click(object sender, EventArgs e)
        {
            axMap1.CursorMode = MapWinGIS.tkCursorMode.cmZoomIn;
        }

        private void toolZoomOut_Click(object sender, EventArgs e)
        {
            axMap1.CursorMode = MapWinGIS.tkCursorMode.cmZoomOut;
        }

        private void toolPan_Click(object sender, EventArgs e)
        {
            axMap1.CursorMode = MapWinGIS.tkCursorMode.cmPan;
        }


    }
}
