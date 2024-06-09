import os
import re

IGNORE_DIRS = ['_Tiled', '.config', '.bin', 'Content', 'python', 'obj']

def get_dependencies(filepath):
    with open(filepath, 'r', encoding='utf-8-sig') as file:
        content = file.read()
    
    # Regex to find using statements
    using_pattern = re.compile(r'^\s*using\s+([\w\.]+);', re.MULTILINE)
    imports = using_pattern.findall(content)
    
    return imports

def scan_directory(directory, ignore_dirs):
    dependencies = {}
    for root, dirs, files in os.walk(directory):
        # Remove ignored directories from the search
        dirs[:] = [d for d in dirs if d not in ignore_dirs]
        for file in files:
            if file.endswith('.cs'):
                filepath = os.path.join(root, file)
                deps = get_dependencies(filepath)
                dependencies[filepath] = deps
    return dependencies

def main():
    project_root = os.path.abspath(os.path.join(os.getcwd(), "../.."))
    dependencies = scan_directory(project_root, IGNORE_DIRS)
    
    for file, deps in dependencies.items():
        print(f"{file}: {deps}")

if __name__ == "__main__":
    main()