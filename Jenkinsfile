pipeline {
    agent any

    options {
        timeout(time: 60, unit: 'MINUTES')
    }

    stages {
        stage('Checkout') {
            steps {
                script {
                    sh '''
                    whoami
                    git config pull.rebase false
                    if [ ! -d \"$PROJECT_PATH\" ]; then
                        git clone git@github.com:DingDingHouse/Slot-FruitCocktail.git /home/ubuntu/Games/Slot-FruitCocktail
                    else
                        echo \"Repository already exists, pulling latest changes.\"
                    fi
                    cd $PROJECT_PATH
                    git config pull.rebase false
                    git config pull.rebase true
                    git fetch --all
                    git reset --hard origin/develop
                    "
                    '''
                }
            }
        }

        stage('Build WebGL') {
            steps {
                script {
                    sh '''
                    echo "1234" | sudo -S -u ubuntu bash -c "
                    /home/ubuntu/Editor/Unity -quit -batchmode -nographics -projectPath /home/ubuntu/Games/Slot-FruitCocktail -executeMethod MyBuilder.WebGLBuilder.Build -logFile /home/ubuntu/Games/Slot-FruitCocktail/build.log
                    "
                    '''
                }
            }
        }

        stage('Push Build to GitHub') {
            steps {
                script {
                    dir("${PROJECT_PATH}") {
                        sh '''
                        echo "1234" | sudo -S -u ubuntu bash -c "
                        git stash -u
                        git checkout main
                        git rm -r -f Builds
                        git rm -r -f Build
                        git rm -f index.html
                        git commit -m 'delete old Builds' || echo 'Nothing to commit'
                        git push origin main

                        git checkout main
                        git checkout develop -- Builds
                        rsync -a --remove-source-files Builds/WebGL/ ./
                        git add -f Build index.html
                        git commit -m 'adding new Builds from Linux' || echo 'Nothing to commit'
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
