using TagTool.Commands.Core;

namespace TagTool.Commands.Tags
{
    static class TagCacheContextFactory
    {
        public static CommandContext Create(CommandContextStack stack, OpenTagCache info)
        {
            var context = new CommandContext(null, info.CacheFile.Name);
            context.AddCommand(new HelpCommand(stack));
            context.AddCommand(new ClearCommand());
            context.AddCommand(new DumpLogCommand());
            context.AddCommand(new EchoCommand());
            context.AddCommand(new SetLocaleCommand());
            context.AddCommand(new DependencyCommand(info));
            context.AddCommand(new ExtractCommand(info));
            context.AddCommand(new ImportCommand(info));
            context.AddCommand(new InfoCommand(info));
            context.AddCommand(new ListCommand(info));
            context.AddCommand(new MapCommand());
            context.AddCommand(new DuplicateTagCommand(info));
            context.AddCommand(new AddressCommand());
            context.AddCommand(new ResourceDataCommand());
            context.AddCommand(new TestCommand(info));
            context.AddCommand(new NullTagCommand(info));
            context.AddCommand(new ListUnreferencedTagsCommand(info));
            context.AddCommand(new ListNullTagsCommand(info));
            context.AddCommand(new GenerateCacheCommand());
            context.AddCommand(new NewTagCommand(info));
            context.AddCommand(new ExportTagsCommand(info));

            if (info.StringIDs != null)
            {
                context.AddCommand(new EditCommand(stack, info));
                context.AddCommand(new ExtractBitmapsCommand(info));
                context.AddCommand(new ImportBitmapCommand(info));
                context.AddCommand(new CollisionGeometryTestCommand(info));
                context.AddCommand(new PhysicsModelTestCommand(info));
                context.AddCommand(new StringIDCommand(info));
                context.AddCommand(new ListStringsCommand(info));
                context.AddCommand(new GenerateLayoutsCommand(info));
                context.AddCommand(new ModelTestCommand(info));
                context.AddCommand(new ConvertPluginsCommand(info));
                context.AddCommand(new GenerateTagNamesCommand(info));
                context.AddCommand(new OpenCacheCommand(stack, info));
                context.AddCommand(new MatchTagsCommand(info));
                context.AddCommand(new ConvertCommand(info));
            }
            return context;
        }
    }
}
