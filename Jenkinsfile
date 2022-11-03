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
		C:\\nuget\\nuget.exe restore .\\CGP.sln 
		'''
	    }
	}
		
	stage('Build') {
            steps {
		print "Building Solution"
                powershell '''
                        if (Test-Path "C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Community\\MSBuild\\Current\\Bin\\amd64\\MSBuild.exe") {
                        } else {
                            Write-Error "Cannot find VS 2019 MSBuild"
                        }
			msbuild $ENV:WORKSPACE\\CGP.sln `
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
	    
    	stage('Archive_Artifacts') {
            steps {
            powershell '''
		Compress-Archive -Path $ENV:WORKSPACE\\Build_Artifacts_Jenkins\\Packages\\* `
		-DestinationPath $ENV:WORKSPACE\\Build_Artifacts_Jenkins\\$ENV:BUILD_NUMBER-$ENV:BRANCH_TAG.zip
		'''
	    }
        }
    }
  
}
