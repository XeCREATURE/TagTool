namespace TagTool.Scripting
{
    /// <summary>
    /// 
    /// </summary>
    public class ScriptGlobal
    {
        /// <summary>
        /// The name of the global.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The value type of the global.
        /// </summary>
        public ScriptValueType Type { get; set; }

        /// <summary>
        /// The index of the global expression within the scenario.
        /// </summary>
        public int ExpressionIndex { get; set; }
    }
}
