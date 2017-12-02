param (
    [string] $project = $(throw "Must provide a project name `"-project blablabla`""),
    [string] $version = $(throw "Must provide a version string `"-version 0.0.0.0`"")
)

function Zip-Files( $zipfilename, $sourcedir )
{
   Add-Type -Assembly System.IO.Compression.FileSystem
   $compressionLevel = [System.IO.Compression.CompressionLevel]::Optimal
   [System.IO.Compression.ZipFile]::CreateFromDirectory($sourcedir,
        $zipfilename, $compressionLevel, $false)
}

Zip-Files "C:\projects\checkinn\$project.$version.zip" "C:\projects\checkinn\$project\bin\Release\"
