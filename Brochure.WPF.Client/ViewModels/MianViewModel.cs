using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Brochure.Core;

namespace Brochure.WPF.Client
{
    public class MianViewModel : ViewModelBase
    {

        public ObservableCollection<MenuItem> Menus { get; set; }


        public override void LoadDataAsync()
        {
            var menu = Mg.Get<IMenus>();
            var key1 = Guid.NewGuid();
            var key2 = Guid.NewGuid();
            var key3 = Guid.NewGuid();
            var fileMenu = new MenuItem("文件", Guid.Empty);


            var newmenu = new MenuItem("新建", key1, 2);
            newmenu.Childs.Add(new MenuItem("项目", key3));

            fileMenu.Childs.Add(newmenu);
            fileMenu.Childs.Add(new MenuItem("打开", key1, 1));

            fileMenu.Childs.Add(new MenuItem("关闭", Guid.NewGuid()));

            var editeMenu = new MenuItem("编辑", Guid.Empty);
            editeMenu.Childs.Add(new MenuItem("转到", key2));
            menu.AddMenuItem(fileMenu);
            menu.AddMenuItem(editeMenu);

            var menu11 = Mg.Get<IMenus>().GetMenuItem(newmenu.Key);
            menu11.Childs.Add(new MenuItem("起始页", key3));
            menu11.Childs.Add(new MenuItem("起始页2", new Guid()));
            Menus = new ObservableCollection<MenuItem>(menu.GetMenuItems());
        }

        public override void InitCommand()
        {
        }
    }


}
