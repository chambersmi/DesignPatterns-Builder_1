using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BuilderExample {
    public class Program {
        static void Main(string[] args) {

            // Low level, incorrect way.
            Console.WriteLine("========================================================================================");
            Console.WriteLine("Incorrect way:\n");
            var hello = "hello";
            var sb = new StringBuilder();
            sb.Append("<p>");
            sb.Append(hello);
            sb.Append("<p>");
            Console.WriteLine(sb);

            // Other example, incorrect way.
            var words = new[] { "hello", "world" };
            sb.Clear();
            sb.Append("<ul>");
            foreach(var word in words) {
                sb.AppendFormat("<li>{0}</li>", word);
            }
            sb.Append("</ul>");
            Console.WriteLine(sb);
            Console.WriteLine("========================================================================================\n");
            Console.WriteLine("Correct way:\n");
            var builder = new HtmlBuilder("ul");
            builder.AddChild("li", "hello").AddChild("li", "world");
            Console.WriteLine(builder.ToString());

        }
    }

    // Create class with HTML elements
    public class HtmlElement {
        public string Name, Text;
        public List<HtmlElement> Elements = new List<HtmlElement>();
        private const int _indentSize = 2;

        public HtmlElement() {
        }

        public HtmlElement(string name, string text) {
            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
            Text = text ?? throw new ArgumentNullException(paramName: nameof(text));
        }

        private string ToStringImpl(int indent) {
            var sb = new StringBuilder();
            var i = new String(' ', _indentSize * indent);
            sb.AppendLine($"{i}<{Name}>");

            if(!string.IsNullOrWhiteSpace(Text)) {
                sb.Append(new string(' ', _indentSize * (indent + 1)));
                sb.AppendLine(Text);
            }

            foreach(var e in Elements) {
                sb.Append(e.ToStringImpl(indent + 1));
            }
            sb.AppendLine($"{i}</{Name}>");
            return sb.ToString();
        }

        public override string ToString() {
            return ToStringImpl(0);
        }
    }

    public class HtmlBuilder {
        private readonly string rootName;

        HtmlElement root = new HtmlElement();

        public HtmlBuilder(string rootName) {
            root.Name = rootName;
        }

        public HtmlBuilder AddChild(string childName, string childText) {
            var e = new HtmlElement(childName, childText);
            root.Elements.Add(e);
            return this;
        }

        public void Clear() {
            root = new HtmlElement { Name = rootName };
        }

        public override string ToString() {
            return root.ToString();            
        }
    }
}
