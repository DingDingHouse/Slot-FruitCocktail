def PROJECT_NAME = "Slot-FruitCocktail"
def UNITY_VERSION = "2022.3.23f1"
def UNITY_INSTALLATION = "/home/ubuntu/Editor/Unity"
def REPO_URL = "git@github.com:DingDingHouse/Slot-FruitCocktail.git"

pipeline {
    agent any

    options {
        timeout(time: 60, unit: 'MINUTES')
    }

    environment {
        PROJECT_PATH = "/home/ubuntu/Games/Slot-FruitCocktail"
    }

    stages {
        stage('Checkout') {
            steps {
                script {
                    sh '''
                    if [ -d "$PROJECT_PATH/.git" ]; then
                        echo "Repository already exists, pulling latest changes."
                        cd $PROJECT_PATH
                        git fetch origin
                        git reset --hard origin/develop
                    else
                        echo "Cloning the repository..."
                        rm -rf $PROJECT_PATH  # Clean up if the directory is present without a .git folder
                        git clone $REPO_URL $PROJECT_PATH
                        cd $PROJECT_PATH
                        git checkout develop
                    fi
                    '''
                }
            }
        }

        stage('Build WebGL') {
            steps {
                script {
                    sh '''
                    ${UNITY_INSTALLATION} -quit -batchmode -nographics -projectPath ${PROJECT_PATH} -executeMethod MyBuilder.WebGLBuilder.Build -logFile ${PROJECT_PATH}/build.log
                    '''
                }
            }
        }

        stage('Push Build to GitHub') {
            steps {
                script {
                    dir("${PROJECT_PATH}") {
                        sh '''
                        git stash -u
                        git checkout main
                        git rm -r -f Builds
                        git rm -r -f Build
                        git rm -f index.html
                        git commit -m "delete old Builds" || echo "Nothing to commit"
                        git push origin main

                        git checkout main
                        git checkout develop -- Builds
                        rsync -a --remove-source-files Builds/WebGL/ ./
                        git add -f Build index.html
                        git commit -m "adding new Builds from Linux" || echo "Nothing to commit"
                        git push origin main
                        git checkout develop
                        git pull origin develop
                        '''
                    }
                }
            }
        }
    }
}
