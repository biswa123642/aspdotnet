#!/usr/bin/env groovy

pipeline {
    agent any

    options {
        // Job timeout
        timeout(time: 1, unit: 'HOURS')
        // Add timestamps to console output
        timestamps()
        // Disable multiple builds at a time for same branch/job
        disableConcurrentBuilds()
        // Only keep 60 builds
        buildDiscarder(logRotator(numToKeepStr: '60'))
    }
	
    stages {
        stage('Checkout') {
	    steps {
		print "SCM Checkout"
		checkout scm
	    }
        } 
		
	stage('Nuget_Restore') {
	    steps {
	    print "Restoring Nuget Packages on sln"
	    powershell '''
		C:\\nuget\\nuget.exe restore .\\CernerComSitecore.sln -PackagesDirectory  .\\packages  
		'''
	    }
	}
		
	stage('Build') {
            steps {
		print "Building Solution"
                powershell '''
                        if (Test-Path "C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\BuildTools\\MSBuild\\15.0\\Bin\\MSBuild.exe") {
                            Set-Alias msbuild ${env:msBuildPath}\\MSBuild.exe -Scope Script
                        } else {
                            Write-Error "Cannot find VS 2017 MSBuild"
                        }
			msbuild $ENV:WORKSPACE\\CernerComSitecore.sln `
			/p:DeployOnBuild=true `
			/p:Configuration=Release `
			/p:WebPublishMethod=FileSystem `
			/p:SkipInvalidConfigurations=true `
			/p:TransformOnBuild=false `
			/p:PublishUrl=$ENV:WORKSPACE\\Build_Artifacts_Jenkins `
			/p:DeployDefaultTarget=WebPublish `
			/p:BuildProjectReferences=true `
			/p:AllowedReferenceRelatedFileExtensions=.pdb
                    '''
            }
        }
   }
  
}
