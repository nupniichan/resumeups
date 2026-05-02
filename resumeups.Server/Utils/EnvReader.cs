namespace resumeups.Server.Utils
{
    public static class EnvReader
    {
        public static void Load()
        {
            var path = Path.Combine(AppContext.BaseDirectory, ".env");
            if (!File.Exists(path))
                path = Path.Combine(Directory.GetCurrentDirectory(), ".env");
            if (!File.Exists(path))
                return;

            foreach (var raw in File.ReadLines(path))
            {
                var line = raw.Trim();
                if (line.Length == 0 || line[0] == '#')
                    continue;
                var eq = line.IndexOf('=');
                if (eq <= 0)
                    continue;
                var key = line[..eq].Trim();
                if (key.Length == 0)
                    continue;
                var val = line[(eq + 1)..].Trim().Trim('"').Trim('\'');
                Environment.SetEnvironmentVariable(key, val);
            }
        }

        public static string Get(string key, string? defaultValue = null) =>
            Environment.GetEnvironmentVariable(key) ?? defaultValue ?? "";
    }
}
