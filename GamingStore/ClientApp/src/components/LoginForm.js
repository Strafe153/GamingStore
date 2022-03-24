import React, { Component } from "react";

export class LoginForm extends Component {
    static displayName = LoginForm.name;

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
                sessionStorage.setItem("token", JSON.stringify(data.token));
                sessionStorage.setItem("role", JSON.stringify(data.role));
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