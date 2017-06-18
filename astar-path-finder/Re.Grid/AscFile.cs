using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Re.Grid
{
    /// <summary>
    /// asc文件 http://resources.arcgis.com/zh-cn/help/main/10.1/index.html#//00120000002s000000
    /// </summary>
    public class AscFile : IDisposable
    {

        #region Read

        /// <summary>
        /// 读取一个asc文件
        /// </summary>
        /// <param name="fileName"></param>
        public AscFile(string fileName)
        {
            var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            _reader = new StreamReader(stream, Encoding.Default);
            ReadHeader();
        }



        public AscFile(string fileName, AscHeader header,int[,] values)
        {
            using (var writer = new StreamWriter(Path.ChangeExtension(fileName, ".prj")))
            {
                writer.WriteLine("Projection    GEOGRAPHIC");
                writer.WriteLine("Datum         WGS84");
                writer.WriteLine("Spheroid      WGS84");
                writer.WriteLine("Units         DD");
                writer.WriteLine("Zunits        NO");
                writer.WriteLine("Parameters");
            }
            var stream = new FileStream(fileName, FileMode.Create);
            StreamWriter _writer = new StreamWriter(stream, Encoding.Default);


            _writer.WriteLine("NCOLS " + header.ColumnCount);
            _writer.WriteLine("NROWS " + header.RowCount);
            _writer.WriteLine("XLLCENTER " + header.XllCenter); //xllcorner:左下角x值,xllcenter:左下角中心点值
            _writer.WriteLine("YLLCENTER " + header.YllCenter);
            _writer.WriteLine("CELLSIZE " + header.CellSize);
            _writer.WriteLine("NODATA_VALUE " + header.NodataValue);

            for (int i = 0; i < header.RowCount; i++)
            {
                var count = header.ColumnCount;
                for (int j = 0; j < count; j++)
                {
                    _writer.Write(values[i, j]);
                    if (j != count - 1)
                    {
                        _writer.Write(" ");
                    }
                    else
                        _writer.WriteLine();
                }
            }
            _writer.Close();
            _writer.Dispose();

        }


        public void ReadIntValues()
        {
            values = new int[header.RowCount, header.ColumnCount]; 
            for(int i = 0; i < header.RowCount; i++)
            {
                string line = _reader.ReadLine();
                char[] spliter = { ' ' };
                string[] tokens =line.Split(spliter, StringSplitOptions.RemoveEmptyEntries);
                for(int j = 0; j < tokens.Length; j++)
                {
                    values[i, j] = Convert.ToInt16(tokens[j]);
                }
                
            }
        }


        private StreamReader _reader;
        public AscHeader header;
        public int[,] values;
        private void ReadHeader()
        {
            header = new AscHeader();
            char[] spliter = { ' ' };
            try
            {
                header.ColumnCount =
                     Convert.ToInt32(_reader.ReadLine().Split(spliter, StringSplitOptions.RemoveEmptyEntries)[1]);
                header.RowCount =
                     Convert.ToInt32(_reader.ReadLine().Split(spliter, StringSplitOptions.RemoveEmptyEntries)[1]);
                header.XllCenter =
                     Convert.ToDouble(_reader.ReadLine().Split(spliter, StringSplitOptions.RemoveEmptyEntries)[1]);
                header.YllCenter =
                     Convert.ToDouble(_reader.ReadLine().Split(spliter, StringSplitOptions.RemoveEmptyEntries)[1]);
                header.CellSize = Convert.ToDouble(_reader.ReadLine().Split(spliter, StringSplitOptions.RemoveEmptyEntries)[1]);
                header.NodataValue =
                     Convert.ToInt32(_reader.ReadLine().Split(spliter, StringSplitOptions.RemoveEmptyEntries)[1]);
            }
            catch (Exception)
            {

                throw new NotSupportedException();
            }
        }


        public int getXIndex(double x)
        {
            double lonGap = Math.Abs(x - header.XllCenter);
            double lonIndex = lonGap / header.CellSize;
            double idx = Math.Round(lonIndex);
            return (int)idx;

        }



        public int getYIndex(double y)
        {
            double YSTART = (header.RowCount - 1) * header.CellSize + header.YllCenter;
            double latGap = Math.Abs(y - YSTART);
            double latIndex = latGap / header.CellSize;
            double idx = Math.Round(latIndex);
            return (int)idx;

        }

        public double getLongitudeByIdx(int idx)
        {
            return header.XllCenter + idx * header.CellSize ;
        }

        public double getLatitudeByIdx(int idx)
        {
            double YSTART = (header.RowCount - 1) * header.CellSize + header.YllCenter;
            return YSTART - idx * header.CellSize;
        }


        #endregion

        #region Dispose

        public void Dispose()
        {

            if (_reader != null)
            {
                _reader.Close();
                _reader.Dispose();
            }
        }

        #endregion
    }
}