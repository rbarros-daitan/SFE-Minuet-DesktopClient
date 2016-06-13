#
# replaces string in msi installer file with real hashed password so that msi can signed
#

$dir = (get-childitem env:CEFDIR).Value
$hashedPasswdFile = $dir + "\hashedPasswd.txt"

if ((Test-Path $hashedPasswdFile) -eq $False) {
	echo "can not find hashed password file in " $hashedPasswdFile
	exit -1
}

$hasedPasswd = [IO.File]::ReadAllText($hashedPasswdFile)

$installerFile = $installerDir + '\Symphony-x86.aip'
if if ((Test-Path $installerFile) -eq $False) {
	echo "can not find installer file in " $installerFile
	exit -1
)

(get-content $installerFile) | foreach-object {$_ -replace "replace-with-hashed-passwd", $hasedPasswd} | set-content $installerFile