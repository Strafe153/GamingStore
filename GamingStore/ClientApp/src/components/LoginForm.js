import React, { Component } from "react";

export class LoginForm extends Component {
    static displayName = LoginForm.name;

    constructor(props) {
        super(props);

        this.state = {
            username: "", 
            password: ""
        };
        
        this.onUsernameChange = this.onUsernameChange.bind(this);
        this.onPasswordChange = this.onPasswordChange.bind(this);
        this.loginUser = this.loginUser.bind(this);
    }

    onUsernameChange(e) {
        this.setState({username: e.target.value});
    }

    onPasswordChange(e) {
        this.setState({password: e.target.value});
    }

    async loginUser(e) {
        e.preventDefault();

        fetch("../api/users/login", {
            method: "POST",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(this.state)
        })
            .then(response => {
                if (response.ok) {
                    return response.json();
                }

                return response.text().then(error => { throw new Error(error) });
            })
            .then(data => {
                sessionStorage.setItem("token", data.token);
                sessionStorage.setItem("role", data.role);

                window.location.href = "/";
            })
            .catch(error => alert(error.message));
    }

    render() {
        return <form onSubmit={this.loginUser}>
            <div className="form-group mb-3">
                <label htmlFor="username" className="control-label">Login:</label>
                <input id="username" className="form-control" type="text" value={this.state.username} onChange={this.onUsernameChange} />
            </div>
            <div className="form-group mb-3">
                <label htmlFor="password" className="control-label">Password:</label>
                <input id="password" className="form-control" type="password" value={this.state.password} onChange={this.onPasswordChange} />
            </div>

            <input className="btn btn-primary" type="submit" value="Login" />
        </form>;
    }
}