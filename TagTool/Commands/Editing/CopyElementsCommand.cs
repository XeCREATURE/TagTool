using System;
using System.Collections;
using System.Collections.Generic;
using TagTool.Serialization;

namespace TagTool.Commands.Editing
{
    class CopyElementsCommand : Command
    {
        private CommandContextStack Stack { get; }

        private OpenTagCache Info { get; }

        private TagInstance Tag { get; }

        private TagStructureInfo Structure { get; set; }

        private object Owner { get; set; }

        #region Element Clipboard
        public static Type ElementType { get; set; } = null;

        public static List<object> Elements { get; set; } = null;
        #endregion

        public CopyElementsCommand(CommandContextStack stack, OpenTagCache info, TagInstance tag, TagStructureInfo structure, object owner)
            : base(CommandFlags.None,
                  "CopyElements",
                  "Copies block elements from one tag to another.",
                  "CopyElements <block name> [count = *] [index = 0]",
                  "Copies block elements from one tag to another.")
        {
            Stack = stack;
            Info = info;
            Tag = tag;
            Structure = structure;
            Owner = owner;
        }

        public override bool Execute(List<string> args)
        {
            if (args.Count > 4)
                return false;


            if (args.Count < 1 || args.Count > 3)
                return false;

            var fieldName = args[0];
            var fieldNameLow = fieldName.ToLower();

            var previousContext = Stack.Context;
            var previousOwner = Owner;
            var previousStructure = Structure;

            if (fieldName.Contains("."))
            {
                var lastIndex = fieldName.LastIndexOf('.');
                var blockName = fieldName.Substring(0, lastIndex);
                fieldName = fieldName.Substring(lastIndex + 1, (fieldName.Length - lastIndex) - 1);
                fieldNameLow = fieldName.ToLower();

                var command = new EditBlockCommand(Stack, Info, Tag, Owner);

                if (!command.Execute(new List<string> { blockName }))
                {
                    while (Stack.Context != previousContext) Stack.Pop();
                    Owner = previousOwner;
                    Structure = previousStructure;
                    return false;
                }

                command = (Stack.Context.GetCommand("Edit") as EditBlockCommand);

                Owner = command.Owner;
                Structure = command.Structure;

                if (Owner == null)
                {
                    while (Stack.Context != previousContext) Stack.Pop();
                    Owner = previousOwner;
                    Structure = previousStructure;
                    return false;
                }
            }

            var count = -1;

            if (args.Count > 1 && args[1] != "*")
            {
                if (!int.TryParse(args[1], out count) || count < 0)
                {
                    Console.WriteLine($"Invalid count specified: {args[1]}");
                    return false;
                }
            }

            var index = 0;

            if (args.Count > 2)
            {
                if (!int.TryParse(args[2], out index) || index < 1)
                {
                    Console.WriteLine($"Invalid index specified: {args[2]}");
                    return false;
                }
            }

            var enumerator = new TagFieldEnumerator(Structure);
            var field = enumerator.Find(f => f.Name == fieldName || f.Name.ToLower() == fieldNameLow);
            var fieldType = field.FieldType;

            if ((field == null) ||
                (!fieldType.IsGenericType) ||
                (fieldType.GetInterface("IList") == null))
            {
                Console.WriteLine("ERROR: {0} does not contain a tag block named \"{1}\".", Structure.Types[0].Name, args[0]);
                while (Stack.Context != previousContext) Stack.Pop();
                Owner = previousOwner;
                Structure = previousStructure;
                return false;
            }

            var blockValue = field.GetValue(Owner) as IList;

            if (blockValue == null)
            {
                Console.WriteLine($"Invalid index specified: {args[0]}");
                return false;
            }

            if (count < 0)
            {
                count = blockValue.Count;
            }

            if ((index + count) < 0 || (index + count) > blockValue.Count)
            {
                Console.WriteLine($"Invalid index: {index}, and count: {count}");
                return false;
            }

            ElementType = field.FieldType.GenericTypeArguments[0];
            Elements = new List<object>();

            for (var i = index; i < (index + count); i++)
                Elements.Add(blockValue[i]);
            
            var itemString = index < 2 ? "element" : "elements";
            Console.WriteLine($"Successfully copied {count} {itemString}.");
            
            while (Stack.Context != previousContext) Stack.Pop();
            Owner = previousOwner;
            Structure = previousStructure;

            return true;
        }
    }
}
