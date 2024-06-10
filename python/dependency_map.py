import os
import re
import graphviz

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

def categorize_dependencies(dependencies):
    categorized_deps = {
        "ThroneGame": set(),
        "ThirdParty": set()
    }
    for deps in dependencies.values():
        for dep in deps:
            if dep.startswith("ThroneGame"):
                categorized_deps["ThroneGame"].add(dep)
            else:
                categorized_deps["ThirdParty"].add(dep)
    return categorized_deps

def map_first_party_dependencies(dependencies):
    first_party_deps = {}
    for filepath, deps in dependencies.items():
        first_party_deps[filepath] = [dep for dep in deps if dep.startswith("ThroneGame")]
    return first_party_deps

def generate_dot_file(first_party_deps, output_file):
    dot = graphviz.Digraph(comment='First-Party Dependency Graph')
    
    for filepath, deps in first_party_deps.items():
        current_namespace = filepath.split('/')[-1].split('.')[0]
        for dep in deps:
            dep_namespace = dep.split('.')[-1]
            dot.edge(current_namespace, dep_namespace)
    
    with open(output_file, 'w') as f:
        f.write(dot.source)

# def main():
#     project_root = os.path.abspath(os.path.join(os.getcwd(), "../.."))
#     dependencies = scan_directory(project_root, IGNORE_DIRS)
    
#     first_party_deps = map_first_party_dependencies(dependencies)
    
#     output_file = 'first_party_dependencies.dot'
#     generate_dot_file(first_party_deps, output_file)
#     print(f"First-party dependency graph saved to {output_file}")

# if __name__ == "__main__":
#     main()