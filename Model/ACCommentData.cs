using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.UI.Xaml.Media.Imaging;
using AcFunVideo.View;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml;

namespace AcFunVideo.Model
{
    public class ACCommentData : AcFunVideoData
    {
        public ACComment Comment { get; set; }
        float width;
        public override void ParseData(string str)
        {
            try
            {
                var comment = JsonConvert.DeserializeObject<ACComment>(str);
                Comment = comment;
            }
            catch (Exception ex)
            {
                throw (new Exception("评论获取失败"));
            }
            //  base.ParseData(str);
        }

        public StackPanel GetCommentView(float width)
        {
            StackPanel lv = new StackPanel();
            if (this.Comment == null)
            {
                return lv;
            }

            foreach (var item in Comment.data.commentList)
            {

                var ncv = ParseEarchFloor(item);
                lv.Children.Add(ncv);
            }
            return lv;

        }

        private NormalCommentView ParseEarchFloor(string floor)
        {


            List<commentContent> temp = new List<commentContent>();
            List<RefCommentView> Reflist = new List<RefCommentView>();
            GetFloorList(floor, Comment, temp);
            NormalCommentView ncv = null;
            bool isRef = false;
            for (int i = temp.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    ncv = GetNormalEarchView(temp[i]);
                }
                else
                {
                    var rcv = GetRefCommentView(temp[i], i);
                    Reflist.Add(rcv);
                }

            }
     
            if (Reflist.Count > 1)
            {

                for (int i = Reflist.Count - 1; i > 0; i--)
                {
                    if (!isRef&&Reflist[i].items.isRef>=2)
                    {
                        isRef = true;
                        Reflist[i].refbutton.Visibility = Visibility.Visible;
                        Reflist[i].rootStackPanel.Visibility = Visibility.Collapsed;
                    }
                    Reflist[i].rootStackPanel.Children.Insert(0, Reflist[i - 1]);
                }
            }
            if (Reflist.Count > 0)
            {
                ncv.rootStackPanel.Children.Insert(0, Reflist[Reflist.Count - 1]);
            }


            return ncv;

        }

        private void GetFloorList(string floor, ACComment ct, List<commentContent> list)
        {
            list.Add(ct.data.commentContentArr["c" + floor]);
            ct.data.commentContentArr["c" + floor].isRef++;
            if (ct.data.commentContentArr["c" + floor].quoteId != "0")
            {
                GetFloorList(ct.data.commentContentArr["c" + floor].quoteId, ct, list);
            }

        }

        private RefCommentView GetRefCommentView(commentContent cc, int i)
        {

            RefCommentView rcv = new RefCommentView();
            rcv.items = cc;
            rcv.usrNameBox.Text = "#" + cc.count + " " + cc.userName;
            var run = new Run();
            run.Text = cc.content;
            rcv.commentParagraph.Inlines.Add(run);
            if (i > 3)
            {
                rcv.underLine.Visibility = Visibility.Visible;
                // rcv.rootBorder.Visibility = Visibility.Collapsed;
                rcv.rootBorder.BorderThickness = new Thickness(0);
                //  rcv.Margin = new Thickness(3,0,3,0);
            }
            else
            {
                rcv.Margin = new Thickness(3, 1, 3, 1);
            }
            return rcv;
        }

        private NormalCommentView GetNormalEarchView(commentContent cc)
        {
            View.NormalCommentView ncv = new View.NormalCommentView();
            if (cc.userImg != null)
            {
                ncv.usrImg.Source = new BitmapImage(cc.userImg);
            }
            else
            {
                ncv.commentStackPanel.Margin = new Thickness(0);
                ncv.usrImg.Width = 1;
            }
            ncv.usrTextBox.Text = "#" + cc.count + " " + cc.userName;
            Windows.UI.Xaml.Documents.Run run = new Windows.UI.Xaml.Documents.Run();
            run.Text = cc.content;
            ncv.commentParagraph.Inlines.Add(run);
            return ncv;
        }
    }
}
