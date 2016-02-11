using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SearchEngine_ver1._1
{
    public partial class Form1 : Form
    {
        int button1_clicked = 0;
        int button2_clicked = 0;
        element[] ht = new element[8111];       //해시 테이블[소수]
        element[] sorted = new element[8111];   //merge sort에 사용되는 임시 저장공간
        string format = "{0, -" + 20 + "}";     //string 출력 형태 format
        //string format2 = "{0, -" + 10 + "}";    //string2 출력 형태 format
        public Form1()
        {
            InitializeComponent();
        }
        //합병함수
        public void merge(int l, int mid, int r)	// 합병 함수 구현
        {
            int i, j, k, m;
            /************************************
            i : 정렬된 왼쪽 배열에 대한 인덱스
            j : 정렬된 오른쪽 배열에 대한 인덱스
            k : 합병 배열에 대한 인덱스
            m : input 배열에 대한 인덱스
            ************************************/

            i = l; j = mid + 1; k = l;	// i, j, k 초기화
            while (i <= mid && j <= r)	// 왼쪽 배열과 오른쪽 배열 비교
            {
                // 알파벳, 특수문자의 개수 비교
                if (ht[i].count >= ht[j].count) { sorted[k++] = ht[i++]; }
                else { sorted[k++] = ht[j++]; }
                // 순서대로 임시 배열에 삽입
            }
            // 남아있는 데이터를 임시 배열에 삽입
            if (i > mid)
            {
                for (m = j; m <= r; m++)
                    sorted[k++] = ht[m];
            }
            else
            {
                for (m = i; m <= mid; m++)
                    sorted[k++] = ht[m];
            }
            // 임시 배열에 있는 데이터( 정렬된 데이터 )를 a에 초기화
            for (m = l; m <= r; m++)
                ht[m] = sorted[m];
        }
        //합병정렬함수
        public void mergesort(int l, int r)	// 합병 정렬 함수 구현
        {
            int mid;	// 배열을 균등하게 분할

            if (l < r)	// l보다 r이 클 경우
            {
                mid = (l + r) / 2;	// mid 초기화

                mergesort(l, mid);	// l부터 mid까지 합병 정렬
                mergesort(mid + 1, r);	// mid+1부터 r까지 합병 정렬

                merge(l, mid, r);	// 정렬된 배열들 합병
            }
        }
        //해싱함수
        public ulong Hash(string key)
        {
            //string->char[]로 하여 각 자리수 연산
            ulong index = 8111;
            char[] chr = key.ToCharArray();

            foreach (char ch in chr)
            {
                int c = ch.ToString().Length;
                while (c > 0)
                {
                    //shift 연산으로 각 자리수 마다 특정 값 연산
                    //같은 유니코드의 조합도 다른 결과값 추출 유도
                    index = (index << 5) + index + ch;
                    c--;
                }
            }
            return index % 8111;
        }
        //로드
        private void Form1_Load(object sender, EventArgs e)
        {

            //textBox2.Text = "**********\t\nword1\r\nword2\r\nword3\r\n형태로 입력\r\r**********";
            this.ActiveControl = label2;
            textBox2.GotFocus += new EventHandler(this.TextGotFocus);
            textBox2.LostFocus += new EventHandler(this.TextLostFocus);

            //해시 테이블 초기화 작업
            for (int i = 0; i < 8111; i++)
            {
                ht[i].count = 0;
                ht[i].data = string.Empty;
            }
        }
        //placeholder를 위한 이벤트1
        public void TextGotFocus(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "input...")
            {
                tb.Text = "";
                tb.ForeColor = Color.Black;
            }
        }
        //placeholder를 위한 이벤트2
        public void TextLostFocus(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                tb.Text = "input...";
                tb.ForeColor = Color.LightGray;
            }
        }

        //Cover Letter Writting
        private void button1_Click(object sender, EventArgs e)
        {
            //작성한 내용 전부 저장
            StreamWriter wr = new StreamWriter("cover_letter.txt");
            wr.WriteLine("\r\n*****작성한 내용*****\r\n");
            wr.Write(textBox1.Text);
            wr.Close();

            //전부 소문자로 변환
            string lower = textBox1.Text.ToLower();
            string[] str_all = lower.Split(new char[]{'\r','\n',' ','–','-','_','+','=','“',':',';','’','`','"', '~', '|', '[', ']', '{', '}', '(', ')', '!', '@', '#', '$', '%', '^', '&', '*', '\t', '.', ',', '?'},StringSplitOptions.RemoveEmptyEntries);

            foreach (string str_part in str_all)
            {
                button1_clicked = 1;
                ulong value = Hash(str_part);   //해싱 값 저장
                int flag = 0;   //찾았는지 판단하는 flag

                //해시 테이블의 해당 인덱스가 비어있으면 값 할당
                if (ht[value].data == string.Empty)
                {
                    ht[value].data = str_part;
                    ht[value].count++;
                }
                else
                {
                    //처리할때 까지 반복
                    while (flag == 0)
                    {
                        //인덱스가 같고 data도 같으면 count만 늘림
                        if (string.Compare(ht[value].data, str_part) == 0)
                        {
                            ht[value].count++;
                            flag = 1;
                        }
                        //인덱스가 같고 data가 다르면 다음 index로 가서 계속
                        else
                        {
                            //value가 배열 범위를 벗어나면 0으로 바꾸고 계속 
                            value++;
                            if (value >= 8111) value = 0;

                            if (ht[value].data == string.Empty)
                            {
                                ht[value].data = str_part;
                                ht[value].count++;
                                flag = 1;
                            }

                            //모든 배열을 다 찾고도 자리가 없다면(원래 인덱스로 돌아오면) 에러 처리
                            if (value == Hash(str_part))
                            {
                                MessageBox.Show("Error");
                                break;
                            }
                        }
                    }
                }
            }
            textBox3.Text = "";
            StreamReader sr1 = new StreamReader("cover_letter.txt");
            textBox3.Text += sr1.ReadToEnd();
            sr1.Close();
            //mergesort(0, 8110); //테이블 내에서 큰 순서대로 sorting
        }
        //Same Words Analysis
        private void button2_Click(object sender, EventArgs e)
        {
            if (button1_clicked == 0)
            {
                button1.PerformClick();
                button1_clicked = 1;
            }

            StreamWriter sw = new StreamWriter("same_word_analysis.txt");
            sw.WriteLine("\r\n*****단어/개수 분석결과*****\r\n");
            for (int i = 0; i < 8111; i++)
            {
                //값이 없는 것은 skip
                if (ht[i].count == 0)
                    continue;
                else
                {
                    //print value & count

                    sw.WriteLine("value = " + string.Format(format, ht[i].data) + "\tcount = " + ht[i].count);
                }
            }
            sw.Close();

            textBox3.Text = "";
            StreamReader sr2 = new StreamReader("same_word_analysis.txt");
            textBox3.Text += sr2.ReadToEnd();
            sr2.Close();
        }
        //Input Ban Words
        private void button3_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter("ban_words.txt");

            sw.Write(textBox2.Text.ToLower());  //소문자로 변환하여 쓰기

            sw.Close();

            textBox3.Text = "";
            textBox3.Text = "입력한 금지어\r\n\r\n";
            StreamReader sr3 = new StreamReader("ban_words.txt");
            textBox3.Text += sr3.ReadToEnd();
            sr3.Close();
        }
        //Compare with Ban Words
        private void button4_Click(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader("ban_words.txt");
            string[] bans = sr.ReadToEnd().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            sr.Close();
            
            StreamWriter sw = new StreamWriter("ban_words_analysis.txt");
            sw.WriteLine("\r\n*****금지어 분석 결과(Warning)*****\r\n");
            //sw.WriteLine("\r\n"+string.Format(format2,"순위")+string.Format(format,"단어")+string.Format(format2,"반복횟수"));
            ulong value;
            int flag =0;
            //int level = 0;
            //순서대로 분석하면서 처리
            foreach (string str in bans)
            {
                value = Hash(str);
                //같은 해시 값이 있다면
                if (ht[value].count != 0)
                {
                    //데이터 비교 후 같으면 경고
                    if (string.Compare(ht[value].data, str) == 0)
                    {
                        //sw.WriteLine(string.Format(format2,level.ToString())+string.Format(format,ht[value].data)+string.Format(format2,ht[value].count));
                        //sw.WriteLine(++level +".\t" + "value = " + string.Format(format, ht[value].data) + "\tcount = " + ht[value].count);
                        sw.WriteLine("value = " + ht[value].data + "\t\tcount = " + ht[value].count);
                        MessageBox.Show("금지어('" + str + "')를 사용하였습니다!!!"); }
                    else
                    {
                        while (flag == 0)
                        {
                            value++;
                            if (value >= 8111) value = 0;

                            if (string.Compare(ht[value].data, str) == 0)
                            {
                                //sw.WriteLine(string.Format(format2, level.ToString()) + string.Format(format, ht[value].data) + string.Format(format2, ht[value].count));
                                //sw.WriteLine(++level + ".\t" + "value = " + string.Format(format, ht[value].data) + "\tcount = " + ht[value].count);
                                sw.WriteLine("value = " + ht[value].data + "\t\tcount = " + ht[value].count);
                                MessageBox.Show("금지어('" + str + "')를 사용하였습니다!!!");
                                flag = 1;
                            }

                            if (value == Hash(str))
                                break;
                        }
                    }

                   }
                }
            sw.Close();

            textBox3.Text = "";
            StreamReader sr4 = new StreamReader("ban_words_analysis.txt");
            textBox3.Text += sr4.ReadToEnd();
            sr4.Close();
        }
        //Word Counting
        private void button5_Click(object sender, EventArgs e)
        {
            if (button1_clicked == 0)
            {
                button1.PerformClick(); //ht에 다시 넣기
                button1_clicked = 1;
            }

            String temp = textBox1.Text;
            int count_str = temp.Length;
            MessageBox.Show("\r\n*****현재까지 쓴 알파벳 개수는 ("+count_str.ToString()+")개입니다.*****\r\n");

            textBox3.Text = "";
            textBox3.Text += "작성한 글자 수 : " + textBox1.Text.Length + "\r\n";
        }
        //Sorting
        private void button6_Click(object sender, EventArgs e)
        {
            //해시 테이블 합병 정렬
            mergesort(0, 8110);
            if(button2_clicked == 0)
            {
                button2.PerformClick();
                button2_clicked = 1;
            }
            MessageBox.Show("정렬 완료(금지어 검색을 하려면, clear버튼을 누르세요.)");

            textBox3.Text = "";
            StreamReader sr2 = new StreamReader("same_word_analysis.txt");
            textBox3.Text += sr2.ReadToEnd();
            sr2.Close();

        }
        //Result
        private void button7_Click(object sender, EventArgs e)
        {
            if (button1_clicked == 0)
            {
                button1.PerformClick(); //ht에 다시 넣기
                button1_clicked = 1;
            }
            button6.PerformClick(); //정렬
            button2.PerformClick(); //다시 분석 및 넣기

            textBox3.Text = "";

            StreamReader sr1 = new StreamReader("cover_letter.txt");
            textBox3.Text += sr1.ReadToEnd();
            sr1.Close();

            textBox3.Text += "작성한 글자 수 : " + textBox1.Text.Length + "\r\n";

            StreamReader sr2 = new StreamReader("same_word_analysis.txt");
            textBox3.Text += sr2.ReadToEnd();
            sr2.Close();

            StreamReader sr3 = new StreamReader("ban_words.txt");
            textBox3.Text += "\r\n입력한 금지어\r\n\r\n";
            textBox3.Text += sr3.ReadToEnd();
            textBox3.Text += "\r\n";
            sr3.Close();

            StreamReader sr4 = new StreamReader("ban_words_analysis.txt");
            textBox3.Text += sr4.ReadToEnd();
            sr4.Close();
        }
        //Clear
        private void button8_Click(object sender, EventArgs e)
        {
            //해시 테이블 초기화 작업
            for (int i = 0; i < 8111; i++)
            {
                ht[i].count = 0;
                ht[i].data = string.Empty;
            }
            textBox3.Text = ""; //결과화면 창 Clear
            button1_clicked = 0;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
