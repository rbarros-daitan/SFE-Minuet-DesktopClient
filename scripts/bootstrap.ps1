Function update_submodules()
{
	echo "update_submodules"
    
    $sync_command = 'git submodule sync'
    $update_command = 'git submodule update --init --recursive'
    
    iex $sync_command
    iex $update_command
}

update_submodules

exit 1