using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Re.Grid
{
    public class ReSampleAscFile
    {

        private string _source;
        private string _dest;

        public ReSampleAscFile(string source,string desifile)
        {
            this._source = source;
            this._dest = desifile;
        }

        public void ReSample(int zoomFactor)
        {
            AscFile asc = new AscFile(_source);
            asc.ReadIntValues();
            asc.Dispose();
            AscHeader header = asc.header;


            AscHeader destHeader = new AscHeader();
            destHeader.CellSize = header.CellSize * zoomFactor;
            destHeader.RowCount = header.RowCount / zoomFactor;
            destHeader.ColumnCount = header.ColumnCount / zoomFactor;
            destHeader.NodataValue = header.NodataValue;

            destHeader.XllCenter = header.XllCenter - header.CellSize / 2 + destHeader.CellSize / 2;
            destHeader.YllCenter = header.YllCenter - header.CellSize / 2 + destHeader.CellSize / 2;

            int[,] destValues = new int[destHeader.RowCount,destHeader.ColumnCount];


            for(int i = 0; i < header.RowCount; i += zoomFactor)
            {
                for(int j = 0; j < header.ColumnCount; j += zoomFactor)
                {

                    int sumSubValue = 0;
                    //滑动矩阵截取数据
                    for(int idelt = 0; idelt < zoomFactor; idelt++)
                    {
                        for(int jdelt = 0; jdelt < zoomFactor; jdelt++)
                        {

                            int v = asc.values[i + idelt, j + jdelt];
                            sumSubValue += v;
                        }
                    }

                    double percent = sumSubValue / zoomFactor / zoomFactor;
                    int ti = i / zoomFactor;
                    int tj = j / zoomFactor;
                    
                    if (percent > 0)
                    {
                        destValues[ti, tj] = 1;
                    }
                    else
                    {
                        destValues[ti, tj] = 0;
                    }


                }
            }

            AscFile destAsc = new AscFile(_dest, destHeader, destValues);





        }




    }
}
