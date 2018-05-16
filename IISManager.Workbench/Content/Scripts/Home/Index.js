var $terminal = null;

function buildIISDomTree() {
    var data = [];
    $.ajax({
        url: '/GetAllIISInfos',
        type: 'GET',
        async: false,
        success: function (result) {
            console.log(result);
            data = result || [];
        }
    });
    return data;
}

function initializeUploader() {
    $("#uploader").fileinput({
        uploadUrl: "/Publish",
        uploadAsync: true,
        minFileCount: 1,
        maxFileCount: 1,
        allowedFileExtensions: ["rar", "zip"],
        elErrorContainer: '#kv-error-1'
    }).on('filebatchpreupload', function (event, data, id, index) {
        //$('#kv-success-1').html('<h4>Upload Status</h4><ul></ul>').hide();
    }).on('fileuploaded', function (event, data, id, index) {
        //var status = data.response.success ? 'successfully' : 'failed';
        //var fname = data.files[index].name,
        //    out = '<li>' + 'Uploaded file # ' + (index + 1) + ' - ' +
        //        fname + ' ' + status + '.' + '</li>';
        //$('#kv-success-1 ul').append(out);
        //$('#kv-success-1').fadeIn('slow');
    });
}

function initializeTermimal() {
    var id = 1;
    $terminal = $('#terminal').terminal(function (command, term) {
        if (command == 'help') {
            term.echo("available commands are mysql, js, test");
        } else if (command == 'test') {
            term.push(function (command, term) {
                if (command == 'help') {
                    term.echo('if you type ping it will display pong');
                } else if (command == 'ping') {
                    term.echo('pong');
                } else {
                    term.echo('unknown command ' + command);
                }
            }, {
                    prompt: 'test> ',
                    name: 'test'
                });
        } else if (command == "js") {
            term.push(function (command, term) {
                var result = window.eval(command);
                if (result != undefined) {
                    term.echo(String(result));
                }
            }, {
                    name: 'js',
                    prompt: 'js> '
                });
        } else if (command == 'mysql') {
            term.push(function (command, term) {
                term.pause();
                $.jrpc("mysql-rpc-demo.php",
                    "query",
                    [command],
                    function (data) {
                        term.resume();
                        if (data.error) {
                            if (data.error.error && data.error.error.message) {
                                term.error(data.error.error.message);
                            } else if (data.error.message) {
                                term.error(data.error.message);
                            } else {
                                term.error('unknow rpc error');
                            }
                        } else {
                            if (typeof data.result == 'boolean') {
                                term.echo(data.result ? 'success' : 'fail');
                            } else {
                                var len = data.result.length;
                                for (var i = 0; i < len; ++i) {
                                    term.echo(data.result[i].join(' | '));
                                }
                            }
                        }
                    },
                    function (xhr, status, error) {
                        term.error('[AJAX] ' + status +
                            ' - Server reponse is: \n' +
                            xhr.responseText);
                        term.resume();
                    });
            }, {
                    greetings: "This is example of using mysql from terminal\n\
you are allowed to execute: select, insert, update and delete from/to table:\n\
    table test(integer_value integer, varchar_value varchar(255))",
                    prompt: "mysql> "
                });
        } else {
            term.echo("unknown command " + command);
        }
    }, {
            greetings: "Welcome to publish terminal"
        });
}

function initializeSignalr() {
    $.connection.hub.stop();
    //$.connection.hub.url = signalrRoot + '/signalr';

    var publishHub = $.connection.publishHub;
    publishHub.client.send = function (message, success) {
        console.log(message, success);
        if ($terminal != null) {
            var color = success == true ? "green" : "red";
            $terminal.echo(message, {
                finalize: function (div) {
                    div.css("color", color);
                }
            });
        }
    }

    $.connection.hub.start().done(function () {
        console.log("connected.");
    }).fail(function () {
        console.log("can not connect.");
    });
}

function initializeIISTreeView() {
    var options = {
        bootstrap2: false,
        showTags: true,
        levels: 5,
        data: buildIISDomTree(),
    };
    $('#treeview').treeview(options);

    $('#context').contextmenu({
        target: '#context-menu',
        before: function (e, context) {
            // execute code before context menu if shown
            console.log(e, context);
            console.log(e.target.dataset.nodeid);
            var selectedNode = $('#treeview').treeview('getNode', e.target.dataset.nodeid);
            vm.cm.id = selectedNode.id;
            vm.cm.status = selectedNode.status;
            return selectedNode.hasContextMenu;
        },
        onItem: function (context, e) {
            var operation = e.target.dataset.operation;
            var handler = contextmenuHandlers[operation];
            if (handler != null) {
                handler(vm.cm.id, e);
            }
        }
    })
}

function execSiteCommand(cmd, id) {
    $.ajax({
        url: '/ExecSiteCommand',
        type: 'POST',
        data: { cmd: cmd, id: id },
        dataType: 'JSON',
        success: function (result) {
            console.log(result);
            if (result.success) {
                vm.cm.status = result.status;
            }
        }
    })
}

const contextmenuHandlers = {
    start: function (id, e) {
        execSiteCommand('start', id);
        e.preventDefault();
    },
    stop: function (id, e) {
        execSiteCommand('stop', id);
        e.preventDefault();
    }
}

var vm = new Vue({
    el: '#container',
    data: function () {
        return {
            title: 'Publish Workbench', //IIS Manager Workbench
            cm: {
                status: -1,
                name: ''
            }
        };
    },
    mounted: function () {
        //initializeIISTreeView();
        initializeUploader();
        initializeTermimal();
        initializeSignalr();
    },
    methods: {
        startSite: function (id, e) {

        },
        stopSite: function (id, e) {

        }
    }
})