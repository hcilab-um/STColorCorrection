//=============================================================================
// Date		: March 11, 2010
//	Author	: Anthony Paul Ortiz
//	License	: CPOL
//=============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Threading;
using System.Threading;
using System.Windows;

namespace PerceptionLib.Hacks
{
  public class MTObservableCollection<T> : ObservableCollection<T>
  {

    public MTObservableCollection()
    {
      collectionChangedHandlers = new Dictionary<NotifyCollectionChangedEventHandler, CollectionChangedWrapperEventData>();
    }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      KeyValuePair<NotifyCollectionChangedEventHandler, CollectionChangedWrapperEventData>[] handlers = collectionChangedHandlers.ToArray();

      if (handlers.Length > 0)
      {
        foreach (KeyValuePair<NotifyCollectionChangedEventHandler, CollectionChangedWrapperEventData> kvp in handlers)
        {
          if (kvp.Value.Dispatcher == null)
          {
            kvp.Value.Action(e);
          }
          else
          {
            kvp.Value.Dispatcher.Invoke(kvp.Value.Action, DispatcherPriority.DataBind, e);
          }
        }
      }
    }

    private Dictionary<NotifyCollectionChangedEventHandler, CollectionChangedWrapperEventData> collectionChangedHandlers;
    public override event NotifyCollectionChangedEventHandler CollectionChanged
    {
      add
      {
        //Dispatcher dispatcher = Dispatcher.CurrentDispatcher; // should always work
        Dispatcher dispatcher = Dispatcher.FromThread(Thread.CurrentThread); // experimental (can return null)...
        collectionChangedHandlers.Add(value, new CollectionChangedWrapperEventData(dispatcher, new Action<NotifyCollectionChangedEventArgs>((args) => value(this, args))));
      }
      remove
      {
        collectionChangedHandlers.Remove(value);
      }
    }

  }
}
