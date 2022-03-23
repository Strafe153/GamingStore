import React, { Component } from "react";

export class LoginForm extends Component {
    constructor(props) {
        super(props);

        this.state = {username: "", password: ""};
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

    async loginUser() {
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
            })
            .catch(error => console.log(error));
    }

    render() {
        return <form onSubmit={this.loginUser}>
            <label htmlFor="username">Login:</label>
            <input id="username" type="text" value={this.state.username} onChange={this.onUsernameChange} />
            <br />
            <label htmlFor="password">Password:</label>
            <input id="password" type="password" value={this.state.password} onChange={this.onPasswordChange} />
            <br />
            <input type="submit" value="Login" />
        </form>;
    }
}