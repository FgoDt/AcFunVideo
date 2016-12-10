using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace AcFunVideo.Utilites
{
    public class IncrementalLoadingCollection<T> : ObservableCollection<T>, ISupportIncrementalLoading
    {
        // 这里为了简单使用了Tuple<IList<T>, bool>作为返回值，第一项是新项目集合，第二项是否还有更多，也可以自定义实体类
        Func<uint, Task<Tuple<List<T>, bool>>> _dataFetchDelegate = null;

        public IncrementalLoadingCollection(Func<uint, Task<Tuple<List<T>, bool>>> dataFetchDelegate)
        {
            if (dataFetchDelegate == null) throw new ArgumentNullException("dataFetchDelegate");

            this._dataFetchDelegate = dataFetchDelegate;
        }

        public bool HasMoreItems { get; set; } = true;

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            if (_busy)
            {
                throw new InvalidOperationException("Only one operation in flight at a time");
                //return AsyncInfo.Run(c =>(c){})

            }

            _busy = true;
            return AsyncInfo.Run((c) => LoadMoreItemsAsync(c, count));
        }

        protected async Task<LoadMoreItemsResult> LoadMoreItemsAsync(CancellationToken c, uint count)
        {
            uint cn = 0;
            try
            {
                this.OnLoadMoreStarted?.Invoke(count);

                // 我们忽略了CancellationToken，因为我们暂时不需要取消，需要的可以加上
                var result = await this._dataFetchDelegate(count);

                var items = result.Item1;

                if (items != null)
                {

                    foreach (var item in items)
                    {
                        cn++;
                        this.Add(item);
                        //await Task.Delay(50);
                    }

                }

                // 是否还有更多
                this.HasMoreItems = result.Item2;

                // 加载完成事件
                OnLoadMoreCompleted?.Invoke((int)cn);

                return new LoadMoreItemsResult { Count = cn };
            }
            finally
            {
                _busy = false;
            }
        }


        public delegate void LoadMoreStarted(uint count);
        public delegate void LoadMoreCompleted(int count);

        public event LoadMoreStarted OnLoadMoreStarted;
        public event LoadMoreCompleted OnLoadMoreCompleted;

        protected bool _busy = false;
    }
}
