using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;

namespace XSemmel.Schema.Parser 
{

    public class XsdExtension : XsdNode, IXsdHasAttribute, IXsdIsType
    {

        public XsdExtension(XmlNode node) : base(node)
        {
            {
                string attr = VisualizerHelper.GetAttr(node, "base");
                if (attr != null)
                {
                    Base = attr;
                }
            }
            {
                string attr = VisualizerHelper.GetAttr(node, "id");
                if (attr != null)
                {
                    Id = attr;
                }
            }
        }

        public string Id { get; private set; }
        public string Base { get; private set; }
        public IXsdNode BaseTarget { get; set; }

        private readonly HashSet<XsdAttribute> _attrs = new HashSet<XsdAttribute>();


        public void AddAtts(XsdAttribute attr)
        {
            _attrs.Add(attr);
        }

        public ICollection<XsdAttribute> GetAttributes()
        {
            return _attrs;
        }

        public override string ToString()
        {
            return string.Format("Extension (base: {0})", Base);
        }

        public override UIElement GetPaintComponent(XsdVisualizer.PaintOptions po, int fontSize)
        {
            StackPanel pnl = new StackPanel();
            if (fontSize <= 0) return pnl;

            pnl.Children.Add(GetPaintTitle(po, fontSize));

            if (po.ExpandTypes)
            {
                if (BaseTarget != null)
                {
                    pnl.Children.Add(BaseTarget.GetPaintComponent(po, fontSize - 1));
                }
                else
                {
                    pnl.Children.Add( new TextBlock { Text = Base, FontSize = fontSize-1} );
                }
            }

            pnl.Children.Add(new TextBlock
            {
                Text = "extends:",
                FontSize = fontSize,
                FontStyle = FontStyles.Italic
            });

            if (_attrs.Count == 0)
            {
                pnl.Children.Add(new TextBlock
                {
                    Text = "(no attributes)",
                    Foreground = Brushes.Gray,
                    FontSize = fontSize
                });
            }
            else
            {
                foreach (XsdAttribute attr in _attrs)
                {
                    pnl.Children.Add(attr.GetPaintComponent(po, fontSize));
                }
            }

            pnl.Children.Add(GetPaintChildren(po, fontSize - 1));

            addMouseEvents(po, pnl, this);

            return new Border
                       {
                           BorderBrush = Brushes.Black, BorderThickness = new Thickness(1), Child = pnl,
                           Margin = new Thickness(5)
                       };
        }


        protected override UIElement GetPaintTitle(XsdVisualizer.PaintOptions po, int fontSize)
        {
            return new TextBlock
            {
                Text = ToString(),
                Background = new LinearGradientBrush(Colors.Chartreuse, Colors.Transparent, 90),
                FontSize = fontSize,
                FontWeight = FontWeights.DemiBold
            };
        }

    }
}