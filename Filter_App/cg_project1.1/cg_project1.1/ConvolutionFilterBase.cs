using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cg_project1._1
{
    public abstract class ConvolutionFilterBase
    {
        public abstract string FilterName
        {
            get;
        }
        public abstract int OffsetRed
        {
            get;
        }
        public abstract int OffsetGreen
        {
            get;
        }
        public abstract int OffsetBlue
        {
            get;
        }
        public abstract int AnchorRow
        {
            get;
        }
        public abstract int AnchorCol
        {
            get;
        }
        public abstract double Divisor
        {
            get;
        }
        public abstract double[,] FilterMatrix
        {
            get;
        }
    }
    
}
