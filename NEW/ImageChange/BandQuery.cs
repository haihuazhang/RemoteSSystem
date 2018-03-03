using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace RemoteSystem
{
    class Calc
    {
        /// <summary>
        /// 算术表达式（例：（b4-b3）/(b4+b3)）
        /// </summary>
        private string expression;
        /// <summary>
        /// 初始化
        /// </summary>
        public Calc()
        {
            expression = "0";
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="exp">算术表达式</param>
        public Calc(string exp)
        {
            expression = exp;
        }
        /// <summary>
        /// 算术表达式的声明与访问
        /// </summary>
        public string Expression
        {
            set
            {
                Expression = value;
            }
            get
            {
                return (expression);
            }
        }
        public int ColumnCounts, LineCounts;

        public bool isExpressionRight = true;
        public bool ishandled=false;
        /// <summary>
        /// 四则运算
        /// </summary>
        /// <returns>返回结果</returns>
        private double[] EvaluateExpression()
        {
            try
            {
                string myExp = expression + "=";             //表达式。。
                Stack<char> optr = new Stack<char>(myExp.Length);    //存放操作符栈。。
                Stack<double[]> opnd = new Stack<double[]>(myExp.Length);      //存放操作数组栈。。
                optr.Push('=');
                int index = 0;                                //字符索引。。
                char c = myExp.ToCharArray()[index++];                   //读取每一个字符。。
                double[] num1, num2;
                while ((c != '=') || (optr.Peek() != '='))
                {
                    if (c == '"')
                    {
                        string band_File = "";
                        while (true)
                        {

                            if ((c = myExp.ToCharArray()[index++]) == '"')
                            {
                                if (band_File.IndexOf(":") == -1)
                                {
                                    //double[] temp = opnd.Pop();
                                    //int Length = temp.GetLength(0);
                                    //opnd.Push(temp);
                                    //double math = Convert.ToDouble(band_File);
                                    //temp = new double[Length];
                                    //for (int i = 0; i < temp.GetLength(0); i++)
                                    //    temp[i] = math;
                                    //opnd.Push(temp);
                                    //c = myExp.ToCharArray()[index++];
                                    //break;
                                }
                                else
                                {
                                    //索引数据
                                    string FileName = Path.GetFileName(band_File);
                                    GetDataByFilename gdbf = new GetDataByFilename();
                                    int i = gdbf.getnumber(Form1.boduan, FileName);
                                    string bandname = band_File.Substring(0, band_File.IndexOf(FileName) - 1);
                                    GetBandByname gbbn = new GetBandByname();
                                    int j = gbbn.getnumber(Form1.boduan[i].Bandsname, bandname, Form1.boduan[i].bands);
                                    //初始化数据并赋值
                                    double[] temp = new double[Form1.boduan[i].ColumnCounts * Form1.boduan[i].LineCounts];
                                    for (int k = 0; k < Form1.boduan[i].ColumnCounts * Form1.boduan[i].LineCounts; k++)
                                    {
                                        temp[k] = Form1.boduan[i].BandsDataD[j, k];
                                    }
                                    ColumnCounts = Form1.boduan[i].ColumnCounts;
                                    LineCounts = Form1.boduan[i].LineCounts;
                                    //将数据压入栈中
                                    opnd.Push(temp);
                                    c = myExp.ToCharArray()[index++];
                                    break;
                                }
                            }
                            band_File += c;
                        }
                        
                    }
                    else if (c != '+' && c != '-' && c != '*' && c != '/' && c != '(' && c != ')'&& c != '=')
                    {
                        string band_File = "";
                        band_File += c;
                        while (true)
                        {
                            if(index+1==myExp.Length)
                                break;
                            c=myExp.ToCharArray()[index++];
                            if (!judge(c))
                                break;
                            band_File += c;
                        }
                        double[] temp = opnd.Pop();
                        int Length = temp.GetLength(0);
                        opnd.Push(temp);
                        double math = Convert.ToDouble(band_File);
                        temp = new double[Length];
                        for (int i = 0; i < temp.GetLength(0); i++)
                            temp[i] = math;
                        opnd.Push(temp);
                        c = myExp.ToCharArray()[index++];
                        
                    }
                    else
                    {
                        bool isJump = false;

                        switch (Precede(optr.Peek(), c))
                        {

                            case '<':
                                optr.Push(c);
                                if ((index + 1) == myExp.Length)
                                {
                                    isExpressionRight = false;
                                    isJump = true;
                                    break;
                                }
                                c = myExp.ToCharArray()[index++];
                                break;
                            case '=':
                                optr.Pop();
                                //if ((index + 1) == myExp.Length)
                                //{
                                //    isExpressionRight = false;
                                //    isJump = true;
                                //    break;
                                //}
                                c = myExp.ToCharArray()[index++];
                                break;
                            case '>':
                                num2 = opnd.Pop();
                                num1 = opnd.Pop();
                                opnd.Push(Operate(num1, optr.Pop(), num2));
                                if (!ishandled)
                                    isJump = true;
                                break;
                            default:
                                break;
                        }
                        if (isJump)
                            break;

                    }
                }
                if (!isExpressionRight)
                {
                    double[] a = new double[100];
                }
                return opnd.Pop();

            }
            catch(Exception)
            {
                
                double[] a = new double[100];
                return a;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool judge(char c)
        {
            bool s=false;
            if (c != '+' && c != '-' && c != '*' && c != '/' && c != '(' && c != ')')
            {
                s = true;
            }
            else
                s = false;
            return s;
        }
        /// <summary>
        /// 判断优先级
        /// </summary>
        /// <param name="optr1">运算符号1</param>
        /// <param name="optr2">运算符号2</param>
        /// <returns></returns>
        private char Precede(char optr1, char optr2)
        {
            //定义一个比较结果(用二维数组存下来)。。
            char[,] optrTable = 
            {
                { '>', '>', '<', '<', '<', '>', '>' },
                { '>', '>', '<', '<', '<', '>', '>' },
                { '>', '>', '>', '>', '<', '>', '>' },
                { '>', '>', '>', '>', '<', '>', '>' },
                { '<', '<', '<', '<', '<', '=', '?' },
                { '>', '>', '>', '>', '?', '>', '>' },
                { '<', '<', '<', '<', '<', '?', '=' }
            };
            int x = 0, y = 0;//申明存符号转化后的整数。。
            //定义一个符号数组。。
            char[] optrs = { '+', '-', '*', '/', '(', ')', '=' };
            for (int i = 0; i < optrs.Length; ++i)
            {
                if (optr1 == optrs[i])
                    x = i;
                if (optr2 == optrs[i])
                    y = i;
            }
            if (optrTable[x, y] == '?')
            {
                throw new Exception("表达式不合法");
            }
            else
            {
                return optrTable[x, y];
            }
        }
        /// <summary>
        /// 计算两波段数据，得出相应结果。。
        /// </summary>
        /// <param name="a">波段a</param>
        /// <param name="optr">运算符号</param>
        /// <param name="b">波段b</param>
        /// <returns></returns>
        private double[] Operate(double[] a, char optr, double[] b)
        {
            double[] result=new double[a.GetLength(0)];
            if (a.GetLength(0) != b.GetLength(0))
            {
                ishandled = false;
            }
            else
            {
                BandMath BM = new BandMath();
                switch (optr)
                {
                    case '+':
                        result = BM.bandPlus(a, b);
                        break;
                    case '-':
                        result = BM.bandMinus(a, b);
                        break;
                    case '*':
                        result = BM.bandMULT(a, b);
                        break;
                    case '/':
                        result = BM.bandDivide(a, b);
                        break;
                    default:
                        break;
                }
                ishandled = true;
            }
            return result;
        }
        public read GetResult()
        {
            read rd = new read();
            rd.bands = 1;
            rd.Bandsname = new string[rd.bands];
            rd.DataType = 4;
            double[] temp = EvaluateExpression();
            rd.LineCounts = LineCounts;
            rd.ColumnCounts = ColumnCounts;
            rd.BandsDataD = new double[1, rd.ColumnCounts * rd.LineCounts];
            rd.BandsData = new int[1, rd.ColumnCounts * rd.LineCounts];
            for (int i = 0; i < rd.BandsDataD.GetLength(1); i++)
            {
                rd.BandsDataD[0, i] = temp[i];
                rd.BandsData[0, i] = (int)rd.BandsDataD[0, i];
            }
            return rd;
        }
    }
}