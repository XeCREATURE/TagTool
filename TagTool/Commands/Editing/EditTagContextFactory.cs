﻿using TagTool.Commands.Bitmaps;
using TagTool.Commands.Models;
using TagTool.Commands.RenderModels;
using TagTool.Commands.RenderMethods;
using TagTool.Commands.Scenarios;
using TagTool.Commands.Unicode;
using TagTool.Commands.VFiles;
using TagTool.Serialization;
using TagTool.TagDefinitions;
using TagTool.TagGroups;
using TagTool.Commands.Animations;

namespace TagTool.Commands.Editing
{
    static class EditTagContextFactory
    {
        public static CommandContext Create(CommandContextStack stack, OpenTagCache info, TagInstance tag)
        {
            var groupName = info.StringIDs.GetString(tag.Group.Name);

            var tagName = $"0x{tag.Index:X4}";

            if (info.TagNames.ContainsKey(tag.Index))
            {
                tagName = info.TagNames[tag.Index];
                tagName = $"(0x{tag.Index:X4}) {tagName.Substring(tagName.LastIndexOf('\\') + 1)}";
            }

            var context = new CommandContext(stack.Context,
                string.Format("{0}.{1}", tagName, groupName));

            switch (tag.Group.Tag.ToString())
            {
                case "vfsl": // vfiles_list
                    EditVFilesList(context, info, tag);
                    break;

                case "unic": // multilingual_unicode_string_list
                    EditMultilingualUnicodeStringList(context, info, tag);
                    break;

                case "bitm": // bitmap
                    EditBitmap(context, info, tag);
                    break;

                case "hlmt": // model
                    EditModel(context, info, tag);
                    break;

                case "mode": // render_model
                    EditRenderModel(context, info, tag);
                    break;

                case "jmad":
                    EditAnimation(context, info, tag);
                    break;

                case "rm  ": // render_method
                case "rmsh": // shader
                case "rmd ": // shader_decal
                case "rmfl": // shader_foliage
                case "rmhg": // shader_halogram
                case "rmss": // shader_screen
                case "rmtr": // shader_terrain
                case "rmw ": // shader_water
                case "rmzo": // shader_zonly
                case "rmcs": // shader_custom
                    EditRenderMethod(context, info, tag);
                    break;

                case "scnr":
                    EditScenario(context, info, tag);
                    break;
            }

            object value = null;

            using (var stream = info.OpenCacheRead())
                value = info.Deserializer.Deserialize(
                    new TagSerializationContext(stream, info.Cache, info.StringIDs, tag),
                    TagStructureTypes.FindByGroupTag(tag.Group.Tag));

            var structure = new TagStructureInfo(
                TagStructureTypes.FindByGroupTag(tag.Group.Tag));

            context.AddCommand(new ListFieldsCommand(info, structure, value));
            context.AddCommand(new SetFieldCommand(stack, info, tag, structure, value));
            context.AddCommand(new EditBlockCommand(stack, info, tag, value));
            context.AddCommand(new AddToCommand(stack, info, tag, structure, value));
            context.AddCommand(new RemoveFromCommand(stack, info, tag, structure, value));
            context.AddCommand(new CopyElementsCommand(stack, info, tag, structure, value));
            context.AddCommand(new PasteElementsCommand(stack, info, tag, structure, value));
            context.AddCommand(new SaveChangesCommand(info, tag, value));
            context.AddCommand(new ExitToCommand(stack));

            return context;
        }

        private static void EditVFilesList(CommandContext context, OpenTagCache info, TagInstance tag)
        {
            VFilesList vfsl;

            using (var stream = info.OpenCacheRead())
                vfsl = info.Deserializer.Deserialize<VFilesList>(
                    new TagSerializationContext(stream, info.Cache, info.StringIDs, tag));

            VFilesContextFactory.Populate(context, info, tag, vfsl);
        }

        private static void EditMultilingualUnicodeStringList(CommandContext context, OpenTagCache info, TagInstance tag)
        {
            MultilingualUnicodeStringList unic;

            using (var stream = info.OpenCacheRead())
                unic = info.Deserializer.Deserialize<MultilingualUnicodeStringList>(
                    new TagSerializationContext(stream, info.Cache, info.StringIDs, tag));

            UnicodeContextFactory.Populate(context, info, tag, unic);
        }

        private static void EditBitmap(CommandContext context, OpenTagCache info, TagInstance tag)
        {
            Bitmap bitmap;

            using (var stream = info.OpenCacheRead())
                bitmap = info.Deserializer.Deserialize<Bitmap>(
                    new TagSerializationContext(stream, info.Cache, info.StringIDs, tag));

            BitmapContextFactory.Populate(context, info, tag, bitmap);
        }

        private static void EditModel(CommandContext context, OpenTagCache info, TagInstance tag)
        {
            Model model;

            using (var stream = info.OpenCacheRead())
                model = info.Deserializer.Deserialize<Model>(
                    new TagSerializationContext(stream, info.Cache, info.StringIDs, tag));

            ModelContextFactory.Populate(context, info, tag, model);
        }

        private static void EditRenderModel(CommandContext context, OpenTagCache info, TagInstance tag)
        {
            RenderModel renderModel;

            using (var stream = info.OpenCacheRead())
                renderModel = info.Deserializer.Deserialize<RenderModel>(
                    new TagSerializationContext(stream, info.Cache, info.StringIDs, tag));

            RenderModelContextFactory.Populate(context, info, tag, renderModel);
        }

        private static void EditAnimation(CommandContext context, OpenTagCache info, TagInstance tag)
        {
            ModelAnimationGraph animation;

            using (var stream = info.OpenCacheRead())
                animation = info.Deserializer.Deserialize<ModelAnimationGraph>(
                    new TagSerializationContext(stream, info.Cache, info.StringIDs, tag));

            AnimationContextFactory.Populate(context, info, tag, animation);
        }

        private static void EditRenderMethod(CommandContext context, OpenTagCache info, TagInstance tag)
        {
            RenderMethodContextFactory.Populate(context, info, tag);
        }

        private static void EditScenario(CommandContext context, OpenTagCache info, TagInstance tag)
        {
            Scenario scenario;

            using (var stream = info.OpenCacheRead())
                scenario = info.Deserializer.Deserialize<Scenario>(
                    new TagSerializationContext(stream, info.Cache, info.StringIDs, tag));

            ScnrContextFactory.Populate(context, info, tag, scenario);
        }
    }
}
