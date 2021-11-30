using System;
using System.Linq;

namespace regexTester
{

    [Serializable]
    public class Sets
    {
        public RGHistItem[] RGHistItems { get; set; }

        public RGHistItemAdded GetOrAddRGHistItem(string template)
        {
            var exItem = RGHistItems.FirstOrDefault(it => it.Template.Equals(template, StringComparison.InvariantCultureIgnoreCase));
            bool isNew = false;
            if (isNew = (exItem == null))
                RGHistItems = new[] { exItem = new RGHistItem { Template = template } }.Concat(RGHistItems).ToArray();
            else
                RGHistItems = new[] { exItem }.Concat(RGHistItems.Except(new[] { exItem })).ToArray();
            if (RGHistItems.Length > 100)
                RGHistItems = RGHistItems.Take(100).ToArray();
            return new RGHistItemAdded(exItem, isNew);
        }
    }

    [Serializable]
    public class RGHistItem
    {
        public string Template { get; set; }

        public override string ToString()
        {
            return Template;
        }

        public override bool Equals(object obj)
        {
            return object.ReferenceEquals(this, obj) || (obj is RGHistItem it ? it.Template.Equals(this.Template, StringComparison.Ordinal) : false);
        }

        public override int GetHashCode()
        {
            return Template?.GetHashCode() ?? 0;
        }
    }

    public class RGHistItemAdded : RGHistItem
    {
        public readonly bool IsNew;
        public RGHistItemAdded(RGHistItem item, bool isNew)
        {
            this.Template = item.Template;
            this.IsNew = isNew;
        }
    }
}