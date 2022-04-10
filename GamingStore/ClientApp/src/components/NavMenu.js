import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import '../NavMenu.css';

export class NavMenu extends Component {
    static displayName = NavMenu.name;

    constructor (props) {
        super(props);

        this.state = {
            collapsed: true,
            isLoggedIn: sessionStorage.getItem('token') != null,
            isAdmin: sessionStorage.getItem('role') === '0'
        };
        
        this.toggleNavbar = this.toggleNavbar.bind(this);
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed
        });
    }

    render () {
        return (
            <header>
                <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
                    <Container>
                    <NavbarBrand tag={Link} to="/">GamingStore</NavbarBrand>
                    <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                    <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
                        <ul className="navbar-nav flex-grow">
                        <NavItem>
                            <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
                        </NavItem>
                        {this.state.isLoggedIn &&
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/users">Users</NavLink>
                            </NavItem>
                        }
                        {this.state.isAdmin &&
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/companies">Companies</NavLink>
                            </NavItem>
                        }
                        {this.state.isAdmin &&
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/devices">Devices</NavLink>
                            </NavItem>
                        }
                        {this.state.isLoggedIn &&
                            <NavItem>
                                <NavLink tag={Link} className="text-danger" to="/logout">Log Out</NavLink>
                            </NavItem>
                        }
                        {!this.state.isLoggedIn &&
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/login">Login</NavLink>
                            </NavItem>
                        }
                        {!this.state.isLoggedIn &&
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/register">Sign Up</NavLink>
                            </NavItem>
                        }
                        </ul>
                    </Collapse>
                    </Container>
                </Navbar>
            </header>
        );
    }
}