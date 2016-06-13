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

$installerFile = 'installer\Symphony-x86.aip'
if ((Test-Path $installerFile) -eq $False) {
	echo "can not find installer file in " $installerFile
	exit -1
}

(get-content $installerFile) | foreach-object {$_ -replace "4A99BAA4D493EE030480AF53BA42EA11CCFB627AB1800400DA9692073D68C522A10A4FD0B5F78525294E51AC7194D55B5EE1D31F", $hasedPasswd} | set-content $installerFile