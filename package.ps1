param (
    [string] $version = $(throw "Must provide a version string `"-version 0.0.0.0`"")
)

function Zip-Files( $zipfilename, $sourcedir )
{
   Add-Type -Assembly System.IO.Compression.FileSystem
   $compressionLevel = [System.IO.Compression.CompressionLevel]::Optimal
   [System.IO.Compression.ZipFile]::CreateFromDirectory($sourcedir,
        $zipfilename, $compressionLevel, $false)
}

Zip-Files "C:\projects\checkinn\CheckINN.WebApi.$version.zip" "C:\projects\checkinn\CheckINN.WebApi\bin\Release\"
