namespace Nemonuri.GenResx.Tasks;

partial class GenResx
{
    private static string CreateGenResxPropsText() => 
$"""
<?xml version="1.0" encoding="utf-8"?>

<!-- This file was generated by: https://github.com/nemonuri/package-metadatas-templates -->

<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003">

</Project>
""";
}