import React, { Component } from "react";

export class Users extends Component {
    static displayName = Users.name;

    constructor(props) {
        super(props);

        this.state = {
            users: [], 
            role: sessionStorage.getItem("role"),
            token: sessionStorage.getItem("token")
        };

        this.getUsers = this.getUsers.bind(this);
        this.showUserInfo = this.showUserInfo.bind(this);
        this.showUserInfoForAdmin = this.showUserInfoForAdmin.bind(this);
    }

    async componentDidMount() {
        await this.getUsers();
    }

    // async componentDidUpdate() {
    //     await this.getUsers();
    // }

    async getUsers() {
        await fetch("../api/users", {
            method: "GET"
        })
            .then(response => {
                if (response.ok) {
                    return response.json();
                }
                
                return response.text().then(error => { throw new Error(error) });
            })
            .then(data => this.setState({users: data}))
            .catch(error => console.log(error.message));
    }

    updateUser(id) {
        window.location.href = `/users/edit/${id}`;
    }

    async deleteUser(id) {
        await fetch(`../api/users/${id}`, {
            method: "DELETE",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json",
                "Authorization": `Bearer ${this.state.token}`
            }
        })
            .then(response => {
                if (response.ok) {
                    return response.json();
                }

                return response.text().then(error => { throw new Error(error) });
            })
            .catch(error => alert(error.message));
    }

    showUserInfo() {
        return <div>
            <table className="table table-bordered">
                <thead>
                    <tr>
                        <th className="text-center">Id</th>
                        <th className="text-center">Username</th>
                        <th className="text-center">Role</th>
                    </tr>
                </thead>
                <tbody>
                {
                    this.state.users.map(user => {
                        return <tr key={user.id}>
                            <td>{ user.id }</td>
                            <td>{ user.username }</td>
                            <td>{ user.role }</td>
                        </tr>;
                    })
                }
                </tbody>
            </table>
        </div>;
    }

    showUserInfoForAdmin() {
        return <div>
            <table className="table table-bordered">
                <thead>
                    <tr>
                        <th className="text-center">Id</th>
                        <th className="text-center">Username</th>
                        <th className="text-center">Role</th>
                        <th className="text-center">Actions</th>
                    </tr>
                </thead>
                <tbody>
                {
                    this.state.users.map(user => {
                        return <tr key={user.id}>
                            <td>{ user.id }</td>
                            <td>{ user.username }</td>
                            <td>{ user.role }</td>
                            <td className="text-center d-flex justify-content-around">
                                <button onClick={() => this.updateUser(user.id)} className="btn btn-sm btn-info text-white">Edit</button>
                                <button onClick={async () => { await this.deleteUser(user.id); await this.getUsers(); }} className="btn btn-sm btn-danger">Delete</button>
                            </td>
                        </tr>;
                    })
                }
                </tbody>
            </table>
        </div>;
    }

    render() {
        if (this.state.role === "0") {
            return this.showUserInfoForAdmin();
        } else {
            return this.showUserInfo();
        }
    }
}